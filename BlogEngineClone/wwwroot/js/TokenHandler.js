const { post } = require("jquery");

class Logindata {
    constructor(Email, Password) {
        this.Email = Email
        this.Password = Password
    }
};

function GetLoginData() {
    const EmailSignin = document.getElementById('EmailSignin').Value;
    const PasswordSignin = document.getElementById('PasswordSignin').Value;

    const logindata = new Logindata(EmailSignin, PasswordSignin);

    fetch('~/Identity/Account/Login', {
        method: 'POST',
        body: JSON.stringify({ logindata }),
        headers: {
            'Content-Type': 'application/json'
        }

    })
        .then(Response => Response.json())
        .then(data => {
            const token = data.token;
            const redirectURL = data.redirectURL;
        })
        .catch(error => console.error('Error:', error));

}


