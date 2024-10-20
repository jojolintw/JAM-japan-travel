//=============================================




function GetAllelements()
{
    const alltext = document.querySelectorAll('.form-control');
    const allcheck = document.querySelectorAll('.form-check-input');
    const allimage = document.querySelectorAll('.img-fluid');
    const doman = "https://localhost:7146";

    // ==================介面顯示===================================================
    const AdminId = document.getElementById('AdminId');
    const AdminName = document.getElementById('AdminName');
    const Account = document.getElementById('Account');
    const Password = document.getElementById('Password')
    const AdminManageStatus = document.getElementById('AdminManageStatus');
    const MemberManageStatus = document.getElementById('MemberManageStatus');
    const IniteraryManageStatus = document.getElementById('IniteraryManageStatus');
    const ShipmentManageStatus = document.getElementById('ShipmentManageStatus');
    const OrderManageStatus = document.getElementById('OrderManageStatus');
    const CouponManageStatus = document.getElementById('CouponManageStatus');
    const CommentManageStatus = document.getElementById('CommentManageStatus');
    const BlogManageStatus = document.getElementById('BlogManageStatus');
    const AdminPhoto = document.getElementById('image');
    // ======================Controls=====================================================
    const insertbtn = document.getElementById('InsertAdmin');
    const alterbtn = document.getElementById('AlterAdmin');
    const submitbtn = document.getElementById('SubmitAdmin');
    const cancelbtn = document.getElementById('CancelAdmin');
    const btnphoto = document.getElementById('btnphoto');
    const deletebtn = document.getElementById('DeleteAdmin');
    const txtKeyword = document.getElementById('Keyword');
    const cardcontainer = document.getElementById('cardcontainer');
    const adminform = document.getElementById('adminform');

    const Imglist = document.getElementById('imglist');
    const NoimageUrl = '/images/Admin/Noimage.png';
    const imageUrl = '/images/Admin/';
    let lastbtnclick = null;
}

function reset() {
    const AdminId = document.getElementById('AdminId');
    const AdminName = document.getElementById('AdminName');
    const Account = document.getElementById('Account');
    const Password = document.getElementById('Password')
    const AdminManageStatus = document.getElementById('AdminManageStatus');
    const MemberManageStatus = document.getElementById('MemberManageStatus');
    const IniteraryManageStatus = document.getElementById('IniteraryManageStatus');
    const ShipmentManageStatus = document.getElementById('ShipmentManageStatus');
    const OrderManageStatus = document.getElementById('OrderManageStatus');
    const CouponManageStatus = document.getElementById('CouponManageStatus');
    const CommentManageStatus = document.getElementById('CommentManageStatus');
    const BlogManageStatus = document.getElementById('BlogManageStatus');
    const AdminPhoto = document.getElementById('image');
    /*=======================================================================================================*/
    const insertbtn = document.getElementById('InsertAdmin');
    const alterbtn = document.getElementById('AlterAdmin');
    const deletebtn = document.getElementById('DeleteAdmin');
    const submitbtn = document.getElementById('SubmitAdmin');
    const cancelbtn = document.getElementById('CancelAdmin');
    const txtKeyword = document.getElementById('Keyword');
    const btnphoto = document.getElementById('btnphoto');
    //===========================================================================================================
    AdminId.value = '';
    AdminName.value = '';
    Account.value = '';
    Password.value = '';
    AdminManageStatus.checked = false;
    MemberManageStatus.checked = false;
    IniteraryManageStatus.checked = false;
    ShipmentManageStatus.checked = false;
    OrderManageStatus.checked = false;
    CouponManageStatus.checked = false;
    CommentManageStatus.checked = false;
    BlogManageStatus.checked = false;
    AdminPhoto.src = '/images/Admin/Noimage.png';
    /*    ============================================================================================================*/
    AdminName.disabled = true;
    Account.disabled = true;
    Password.disabled = true;
    AdminManageStatus.disabled = true;
    MemberManageStatus.disabled = true;
    IniteraryManageStatus.disabled = true;
    ShipmentManageStatus.disabled = true;
    OrderManageStatus.disabled = true;
    CouponManageStatus.disabled = true;
    CommentManageStatus.disabled = true;
    BlogManageStatus.disabled = true;

/*    ==========================================================================================================*/
    insertbtn.disabled = false;
    alterbtn.disabled = true;
    deletebtn.disabled = true;
    submitbtn.disabled = true;
    txtKeyword.value = '';
    btnphoto.value = '';
    btnphoto.disabled = true;
}
function enabled() {
    const AdminId = document.getElementById('AdminId');
    const AdminName = document.getElementById('AdminName');
    const Account = document.getElementById('Account');
    const Password = document.getElementById('Password')
    const AdminManageStatus = document.getElementById('AdminManageStatus');
    const MemberManageStatus = document.getElementById('MemberManageStatus');
    const IniteraryManageStatus = document.getElementById('IniteraryManageStatus');
    const ShipmentManageStatus = document.getElementById('ShipmentManageStatus');
    const OrderManageStatus = document.getElementById('OrderManageStatus');
    const CouponManageStatus = document.getElementById('CouponManageStatus');
    const CommentManageStatus = document.getElementById('CommentManageStatus');
    const BlogManageStatus = document.getElementById('BlogManageStatus');
    const AdminPhoto = document.getElementById('image');
    /*=======================================================================================================*/
    const insertbtn = document.getElementById('InsertAdmin');
    const alterbtn = document.getElementById('AlterAdmin');
    const deletebtn = document.getElementById('DeleteAdmin');
    const submitbtn = document.getElementById('SubmitAdmin');
    const cancelbtn = document.getElementById('CancelAdmin');
    const txtKeyword = document.getElementById('Keyword');
    const btnphoto = document.getElementById('btnphoto');
    //===========================================================================================================
    /*    ============================================================================================================*/
    AdminName.disabled = false;
    Account.disabled = false;
    Password.disabled = false;
    AdminManageStatus.disabled = false;
    MemberManageStatus.disabled = false;
    IniteraryManageStatus.disabled = false;
    ShipmentManageStatus.disabled = false;
    OrderManageStatus.disabled = false;
    CouponManageStatus.disabled = false;
    CommentManageStatus.disabled = false;
    BlogManageStatus.disabled = false;
    submitbtn.disabled = false;
    btnphoto.disabled = false;
}


function addCardEvent() {

    const allcards = document.querySelectorAll('.card');
    allcards.forEach(card => {
        card.addEventListener('click', async function () {

            const AdminId = document.getElementById('AdminId');
            const AdminName = document.getElementById('AdminName');
            const Account = document.getElementById('Account');
            const Password = document.getElementById('Password')
            const AdminManageStatus = document.getElementById('AdminManageStatus');
            const MemberManageStatus = document.getElementById('MemberManageStatus');
            const IniteraryManageStatus = document.getElementById('IniteraryManageStatus');
            const ShipmentManageStatus = document.getElementById('ShipmentManageStatus');
            const OrderManageStatus = document.getElementById('OrderManageStatus');
            const CouponManageStatus = document.getElementById('CouponManageStatus');
            const CommentManageStatus = document.getElementById('CommentManageStatus');
            const BlogManageStatus = document.getElementById('BlogManageStatus');
            const AdminPhoto = document.getElementById('image');

            reset();
            // 從 data-adminid 屬性中取得 AdminId
            const adminId = this.getAttribute('data-adminid');
            const response = await fetch(`https://localhost:7146/AdminAPI/GetData/${adminId}`,
                {
                    method: "Get"
                })

            const data = await response.json();

            AdminId.value = data.adminId;
            AdminName.value = data.adminName;
            Account.value = data.account;
            Password.value = data.password;
            AdminManageStatus.checked = data.adminManageStatus ? true : false;
            MemberManageStatus.checked = data.memberManageStatus ? true : false;
            IniteraryManageStatus.checked = data.initeraryManageStatus ? true : false;
            ShipmentManageStatus.checked = data.shipmentManageStatus ? true : false;
            OrderManageStatus.checked = data.orderManageStatus ? true : false;
            CouponManageStatus.checked = data.couponManageStatus ? true : false;
            CommentManageStatus.checked = data.commentManageStatus ? true : false;
            BlogManageStatus.checked = data.blogManageStatus ? true : false;
            if (data.imagePath == null) {
                AdminPhoto.src = '/images/Admin/Noimage.png';
            }
            else {
                AdminPhoto.src = `/images/Admin/${data.imagePath}`;
            }
            window.scrollTo({ top: 0, behavior: 'smooth' });

            alterbtn.disabled = false;
            deletebtn.disabled = false;
        });
    });
}
/*======================增加按鈕事件==================================================================*/
function addbtnEvent() {
    let lastbtnclick;
    //==========================增加新增按鈕===============================================================
    const insertbtn = document.getElementById('InsertAdmin');
    insertbtn.addEventListener('click', (event) => {
        lastbtnclick = event.target;
      
        reset();
        enabled();
        insertbtn.disabled = true;
    })








    //==============Submit按鈕==========================================================================================================
    const submitbtn = document.getElementById('SubmitAdmin');
    submitbtn.addEventListener('click', async () => {
        let form = new FormData(adminform);
        if (lastbtnclick.id == 'InsertAdmin') {
            const response = await fetch(`https://localhost:7146/AdminAPI/InsertAdmin`,
                {
                    method: "Post",
                    body: form
                })
            if (response.ok) {
                const alladmins = await response.json();

                const elealladmins = alladmins.map(ad => {
                    return (`<div class="col-md-6 col-lg-3">
                                             <div class="card" style="width: 300px;height:400px" data-adminid="${ad.adminId}">
                                             <input type="text" value="${ad.adminId}" />
                                             <img class="img-fluid" src="/images/Admin/${ad.imagePath}" alt="" style="height:280px;padding:20px"  onerror="this.onerror=null; this.src='/images/Admin/Noimage.png';">
                                             <div class="card-body">
                                             <h5 class="card-title">${ad.adminName}</h5></div></div></div>`)
                })
                cardcontainer.innerHTML = elealladmins.join("");
                addCardEvent();
            }
            insertbtn.disabled = false;
        }
        /*        ============================================================================================================================================================*/
        else if (lastbtnclick.id == 'AlterAdmin')
        {

        }




        submitbtn.disabled = true;
        reset();
    })



    //==============註冊取消按鈕===============================================================================================
    const cancelbtn = document.getElementById('CancelAdmin');
    cancelbtn.addEventListener('click', () => {
        reset();
    })






}


/*=================Change事件、圖片上傳=========================================================================*/

function addChangeEvent()
{
    //===================CheckBox事件=================================
    const allcheck = document.querySelectorAll('.form-check-input');

    allcheck.forEach(chb => {
        chb.addEventListener('change', () => {
            chb.value = chb.checked ? "true" : "false";
        })
    })
   //================圖片預覽====================================================
    const btnphoto = document.getElementById('btnphoto');
    const AdminPhoto = document.getElementById('image');
    const submitbtn = document.getElementById('SubmitAdmin');

    btnphoto.addEventListener('change', async (event) => {
        let file = event.target.files[0];
        const allowtype = 'image.*';
        if (file.type.match(allowtype)) {
            const reader = new FileReader();
            reader.onload = function () {
                let dataURL = reader.result;
                AdminPhoto.src = `${dataURL}`;
            }
            reader.readAsDataURL(file);
            submitbtn.disabled = false;
        }
        else {
            alert('圖片格式錯誤');
            submitbtn.disabled = true;
        }

    })


}
