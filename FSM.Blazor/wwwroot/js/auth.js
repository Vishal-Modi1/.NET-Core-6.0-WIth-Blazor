﻿export function SignIn(email, password, redirect) {

    var url = "/api/auth/signin";

    // Data to send
    var data = {
        email: email,
        password: password,
        timeZone: GetTimeZone()
    };


    $.ajax({

        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        contentType : 'application/json',
        success: function (response) {

            if (response.status == 200) {

                location.replace("/");
            }
            else {
                return dotnetReferenceObject.invokeMethodAsync("ManageLoginResponse", response.message);
            }
        }
    });
}

//var BlazorUniversity = BlazorUniversity || {};
//BlazorUniversity.startRandomGenerator = function (dotNetObject) {
//    setInterval(function () {
//        debugger
//        let text = Math.random() * 1000;
//        console.log("JS: Generated " + text);
//        dotNetObject.invokeMethodAsync('AddText', text.toString());
//    }, 1000);
//};

var dotnetReferenceObject = null
export function ManageLoginResponse(dotNetHelper) {

    dotnetReferenceObject = dotNetHelper;
};

export function ManageDocumentDownloadResponse(dotNetHelper, id) {

    dotnetReferenceObject = dotNetHelper;

    dotnetReferenceObject.invokeMethodAsync("ManageDocumentDownloadResponse", id);
};

export function SignOut(redirect) {

    var url = "/api/auth/signout";
    var xhr = new XMLHttpRequest();

    // Initialization
    xhr.open("GET", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    // Catch response
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) // 4=DONE 
        {
            console.log("Call '" + url + "'. Status " + xhr.status);
            if (redirect)
                location.replace(redirect);
        }
    };

    // Call API
    xhr.send();
}

export function GetTimeZone() {

    const split = new Date().toString().split(" ");
    const timeZone = split.slice(-3).join(' ')

    return timeZone;
}

export async function downloadFileFromStream(fileName, contentStreamReference, id) {

    debugger
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url, id);

    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url, id) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

export function copyTextToClipboard(text) {

    var dummy = document.createElement("textarea");

    document.body.appendChild(dummy);

    dummy.value = text;
    dummy.select();
    document.execCommand("copy");
    document.body.removeChild(dummy);

}