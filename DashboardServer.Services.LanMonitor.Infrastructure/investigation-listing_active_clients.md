# Goal

Find a way to programattically grab stats from my router, an ASUS RT-AC68U, such as the connected clients and internet connectivity status.

## Goal 1: get a list of connected clients

1. Enable SSH in GUI, you'll need to paste your public key and then you can use `ssh admin@<router-ip-addresss>` to get access
2. Google for a bit about what I'm looking for
3. Find a post which points me at `/var/lib/misc/dnsmasq.leases`

File schema for anyone interseted (Involved some googling to find, thank you archived emails!), from left to right:
1. Lease expiry time in seconds
2. Mac address
3. Ip address
4. Host name
5. Client id

## Goal 2: Getting connection status of the router

The other files in `/var/lib/misc/` didn't really have anything which, at a glance (and an unwise zcat), looked useful.

Looking through the [dd-wrt wiki](https://forum.dd-wrt.com/wiki/index.php/Main_Page) I found out about `nvram` which allows you to explore and modify certain variables. I used `nvram show > nvram.txt` to get a list and explore. But at 2258 entries, I needed another way of finding what I'm after.

The [dd-wrt script examples](https://wiki.dd-wrt.com/wiki/index.php/Script_Examples) revealed something interesting: asp pages. For the router's site. Someone better versed in Linux would have known to look in `/www` way sooner than I found it. But I'm here now. I used scp to copy the ASP files so I can explore them in VS code. Using the actual site as a reference, I looked for how the `Connected` status shows up. This led me to also `scp` across the .js files, which in turn led me to `scp` across the `.xml` files. The one I'm most intereseted in is `ajax_status.xml`. By the look of things, the site makes a request for the XML and the XML executes `nvram get` to fetch variables. Out of the 65 entries, `link_internet` looks promising.

Checking `nvram get link_internet`, I got a value of `2`. Which I'll assume is router-speak for `Connected`. Looking in `state.js` this assumption appears to be correct:

```
this.hasInternet = false;
if (_link_auxstatus == "1") {
	this.hint = "<#658#>";
	this.link = "/error_page.htm?flag=1";
	this.className = "_disconnected";
}
else if (_link_status == "2" && _link_sbstatus == "0") {
	this.link = "";
	if (dualwan_enabled && active_wan_unit != unit && (wans_mode == "fo" || wans_mode == "fb")) {
		this.hint = "<#2437#>";
		this.className = "_standby";
	}
	else if (link_internet == "2") {
		this.hint = "<#133#>";
		this.className = "_connected";
		this.hasInternet = true;
	}
}
```

So we now know that we can check that the router is connected with `link_internet`. We also have some additional properties we can use to figure out **why** it isn't connected.

## What else can we find out?

Let's return back to my original plan, building an API to serve information to my dashboard. I can use `nvram` to fetch some useful information, such as the SSIDs of my router. `wl0_ssid` for the 2.4GHz band and `wl1_ssid` for the 5G band.

Passwords for the device are also here, in plain text. Let's **not** grab those.

## Improving the client list

The lease list was a good start. However on the UI for the router, the device list has other pieces of information which would be quite nice to display. For example, the type of interface and devices with custom names (renamed in the UI). So it's time to go looking in the `nvram` yet again!

For custom names, `custom_clientlist` has a map of name to MAC address. But that's only part of the story. Time to go digging around in the asp files. Searching for the id of the client list (`clientlist_viewlist_content`) led to the function `create_clientlist_listview()` in `client_function.js`. The `clientList` array is where the list gets populated by the variable `originData` which has two properties with inline methods.

```
var originData = {
    fromNetworkmapd: [<% get_clientlist(); %>],
    nmpClient: [<% get_clientlist_from_json_database(); %>], //Record the client connected to the router before.
init: true
}
```

It appears there's a database somewhere. This could be useful. Google found a useful post on a [forum](http://www.snbforums.com/threads/release-freshjr-adaptive-qos-improvements-custom-rules-and-inner-workings.36836/post-467913) leading to two files:
* `/jffs/nmp_cl_json.js` - a list of every device which has connected to the router. The json is keyed by the mac address and the object contains: `type` (not sure what this means, it's mostly 0 for my devices), `mac address`, `name` and `vendor`
* `/tmp/clientlist.json` - a list of currently connected devices, grouped by interface. Each entry is keyed by the `mac address` with an `ip address` in the class. The wireless entries also has an RSSI value, which is an indicator of signal strength

Combining these two json files should give me everything I'm interested in. At this point I don't really need the leases file. It has some interesting additional information but it's not stuff I'm interested in on the UI.
