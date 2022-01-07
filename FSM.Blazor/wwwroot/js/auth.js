export function SignIn(email, password, redirect) {

    var url = "/api/auth/signin";
    var xhr = new XMLHttpRequest();

    // Initialization
    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    // Catch response
    xhr.onreadystatechange = function () {

        if (xhr.status === 200) // 4=DONE
        {
            console.log("Call '" + url + "'. Status " + xhr.status);
            if (redirect)
                location.replace(redirect);
        }
        else if (xhr.status === 401) {

           
        }
    };

    // Data to send
    var data = {
        email: email,
        password: password
    };

    // Call API
    xhr.send(JSON.stringify(data));
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