const host_name = "BrowserNativeMessages";
var port = null;

connectToNative();

browser.tabs.onUpdated.addListener(NewTab);
  
//  -  -  -  -  -  -  -  -  -  -  -

function NewTab(tabId, changeInfo, tabInfo) {
    port.postMessage({"Tab" : tabInfo});
}

function connectToNative() {
    console.log('Connecting to native host: ' + host_name);
    port = chrome.runtime.connectNative(host_name);
    console.log('Adding listener onNativeMessage');
    try {port.onMessage.addListener(onNativeMessage);}
    catch(error) {
        console.error("addListener(onNativeMessage) error: "+ error);
        }
    console.log('Adding listener onDisconnected');
    try {port.onDisconnect.addListener(onDisconnected);}
    catch(error) {
        console.error("addListener(onDisconnected) error: "+ error);
        }
    console.log('Posting message');
    message = {"text" : "connected"};
    try {port.postMessage(message);}
    catch(error) {
        console.error("postMessage error: "+ error);
        }
    console.log('Message posted');
}

function onNativeMessage(message) {
    console.log('recieved message from native app: ' + JSON.stringify(message));
}

function onDisconnected() {
    if (port.error !== null)
        console.log("port.error: " + port.error);
    if (chrome.runtime.lastError !== null)
        console.log("chrome.runtime.lastError"+chrome.runtime.lastError);
    console.log('disconnected from native app.');
    port = null;
}
