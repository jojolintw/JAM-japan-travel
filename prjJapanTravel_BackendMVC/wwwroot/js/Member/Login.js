
const mainContent = document.getElementById('mainContent');
const linkAdmin = document.getElementById('linkAdmin');
   const linkMmeber = document.getElementById('linkMmeber');
/*    =======================================================================*/






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
         }
     })

     //===========================================================================================
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
    }
})











