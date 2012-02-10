WazSmoothStreamingPlayer is a simple Windows Azure application that displays
videos stored in blob storage and plays them via the Windows Azure CDN using
a Smooth Streaming player.

**NOTE**: Smooth Streaming is available on the Windows Azure CDN as part of a
private CTP. Unless you're part of that CTP, you'll be unable to use this
method for playing Smooth Streaming videos.

Usage
-----
Add your blob storage connection string, container name, and CDN endpoint to
`ServiceConfiguration.*.cscfg`. Then run the app and add videos to the
container. Make sure the container already exists and is configured for public
access.

See Also
--------
For a slick way to upload videos to blob storage, see 
[WindowsAzurePublishPlugin](https://github.com/smarx/WindowsAzurePublishPlugin).