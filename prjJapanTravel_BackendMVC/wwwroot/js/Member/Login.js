
const mainContent = document.getElementById('mainContent');
const linkAdmin = document.getElementById('linkAdmin');
const linkMmeber = document.getElementById('linkMmeber');
const linkLogin = document.getElementById('linkLogin');
const linkLogout = document.getElementById('linkLogout');

//登入頁面相關
const Loginbutton = document.getElementById('btnsubmit');
const ErrorAcc = document.getElementById('divErrA');
const ErrorPass = document.getElementById('divErrP');
const loginform = document.getElementById('loginform');
const txts = document.querySelectorAll('.form-control')

/*    =======================================================================*/
function addloginPageEvent()
{
    const Loginbutton = document.getElementById('btnsubmit');
    const loginform = document.getElementById('loginform');
    const ErrorAcc = document.getElementById('divErrA');
    const ErrorPass = document.getElementById('divErrP');
    const txts = document.querySelectorAll('.form-control')
    const btnLoginDemo1 = document.getElementById('btnLoginDemo1');
    const btnLoginDemo2 = document.getElementById('btnLoginDemo2');
    const inputAccooumt = document.getElementById('inputAccooumt');
    const inputPassword = document.getElementById('inputPassword');
    //===============================================================================
    txts.forEach(txt => {
        txt.addEventListener('click', () => {
            ErrorAcc.textContent = '';
            ErrorPass.textContent = '';
        })
    })
    Loginbutton.addEventListener('click', async (event) => {
        const Loginform = new FormData(loginform);
        const response = await fetch(`https://localhost:7146/Login/LoginPage`, {
            method: "Post",
            body: Loginform
        });
        const data = await response.json();
        if (data.result === "Noaccount") {
            ErrorAcc.textContent = data.message;
        }
        else if (data.result === "Nopassword") {
            ErrorPass.textContent = data.message;
        }
        else if (data.result === "OK") {
            const secondresponse = await fetch(`https://localhost:7146/Login/LoginToHome`,
                {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
            mainContent.innerHTML = await secondresponse.text();
        }
    })
    btnLoginDemo1.addEventListener('click', () => {
        inputAccooumt.value = 'winne1946';
        inputPassword.value = 'w12345';
    })
    btnLoginDemo2.addEventListener('click', () => {
        inputAccooumt.value = 'bread1234';
        inputPassword.value = 'b12345';
    })
}

//==========去登入頁===============================================================
linkLogin.addEventListener('click', async () =>
{
    const response = await fetch(`https://localhost:7146/Login/LoginPage`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
    mainContent.innerHTML = await response.text();
    addloginPageEvent();
})
//=============登出==================================================================
linkLogout.addEventListener('click', async () =>
{
    const response = await fetch(`https://localhost:7146/Login/Loginout`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
    mainContent.innerHTML = await response.text();
})



//==========去管理員頁面========================================================================
     linkAdmin.addEventListener('click', async () =>
     {
         const response = await fetch(`https://localhost:7146/Admin/AccessAdmin`,
             {
                 method: 'GET',
                 headers: {
                     'Content-Type': 'application/json'
                 }
         })

         const result = await response.json();
         if (result.success === false && result.errormessage === '未登入') {
             alert('請先登入');
         }
         if (result.success === false && result.errormessage === '無權限') {
             alert('沒有權限');
         }
         if (result.success === true) {
             const secondresponse = await fetch(`https://localhost:7146/Admin/AdminList`,
                 {
                     method: 'GET',
                     headers: {
                         'Content-Type': 'application/json'
                     }
                 })

             mainContent.innerHTML = await secondresponse.text();
             addChangeEvent();
             addbtnEvent();
             addCardEvent(); 
             addDemoEvent();
         }
     })

     //=============去會員頁面==============================================================================
linkMmeber.addEventListener('click', async () => {
    const response = await fetch(`https://localhost:7146/Member/AccessMember`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })

    const result = await response.json();
    if (result.success === false && result.errormessage === '未登入') {
        alert('請先登入');
    }
    if (result.success === false && result.errormessage === '無權限') {
        alert('沒有權限');
    }
    if (result.success === true) {
        const secondresponse = await fetch(`https://localhost:7146/Member/MemberList`,
            {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
        mainContent.innerHTML = await secondresponse.text();
        addSelectitems();
        addMemberbtnEvent();
        addMemberpicSearch();
        addrowEvent();
        addMemberDemo();
    }
})











