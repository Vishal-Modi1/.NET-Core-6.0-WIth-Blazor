export function SignIn(email, password, redirect) {

    
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

                location.replace(redirect);
            }
            else {
                return dotnetReferenceObject.invokeMethodAsync("ManageLoginResponse", response.message);
            }
        }
    });
}

export function ChangeCompany(userId, companyId) {

    var url = "/api/auth/changecompany?userId=" + userId + "&companyId=" + companyId + "&timezone=" + GetTimeZone();

    // Data to send
    var data = {
        userId: userId,
        companyId: companyId,
        timezone: GetTimeZone()
    };

    $.ajax({

        url: url,
        type: 'GET',
        contentType: 'application/json',
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

export function RefreshToken(RefreshToken, UserId) {

    var url = "/api/auth/refreshtoken?refreshToken=" + RefreshToken + "&userId=" + UserId;
    
    $.ajax({

        url: url,
        type: 'GET',
        contentType: 'application/json',
        success: function (response) {
           
                return dotnetReferenceObject.invokeMethodAsync("ManageRefreshTokenResponse", response);
        },
        error: function (errorInfo) {

            debugger
        }
    });
}


var dotnetReferenceObject = null
export function SetDotNetObject(dotNetHelper) {

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

    //const split = new Date().toString().split(" ");
    //const timeZone = split.slice(-3).join(' ')

    //return timeZone;

   return  Intl.DateTimeFormat().resolvedOptions().timeZone;
}

export async function downloadFileFromStream(fileName, contentStreamReference, id) {

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

export function ReloadVideo() {

    document.getElementById("videoTagId").load();
}