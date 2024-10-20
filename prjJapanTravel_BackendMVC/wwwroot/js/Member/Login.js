
   const linkAdmin = document.getElementById('linkAdmin');
   const mainContent = document.getElementById('mainContent');

/*    =======================================================================*/
     linkAdmin.addEventListener('click', async () =>
     {
         //await loadPage(`https://localhost:7146/Admin/List`);
         //const html = await response.text();
         //document.getElementById('mainContent').innerHTML = html;

         const response = await fetch(`https://localhost:7146/Admin/GetDatas`,
             {
                 method: 'GET',
                 headers: {
                     'Content-Type': 'application/json'
                 }
         })

         const data = await response.json();
         if (data.success === false && data.errormessage === '未登入') {
             alert('請先登入');
         }
         if (data.success === false && data.errormessage === '無權限') {
             alert('沒有權限');
         }
         if (data.success === true) {
             console.log(data.success);
             const secondresponse = await fetch(`https://localhost:7146/Admin/AdminList`,
                 {
                     method: 'GET',
                     headers: {
                         'Content-Type': 'application/json'
                     }
                 })

             mainContent.innerHTML = await secondresponse.text();
             addCardEvent();
             addbtnEvent();
             addChangeEvent();
         }
    })











