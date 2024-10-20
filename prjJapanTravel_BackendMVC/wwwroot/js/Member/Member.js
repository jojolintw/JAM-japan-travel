
const selectareas = document.getElementById('selectareas');
const selectlevel = document.getElementById('selectlevel');
const membertable = document.getElementById('membertable');

//========所有控制項=====================================================================================================
const MemberId = document.getElementById('MemberId');
const MemberName = document.getElementById('MemberName');
const Phone = document.getElementById('Phone');
const Email = document.getElementById('Email');
const MemberPassword = document.getElementById('Password');
const sexm = document.getElementById('sexm');
const sexfm = document.getElementById('sexfm');
const Birthday = document.getElementById('Birthday');
const MemberKeyword = document.getElementById('Keyword');
//============================================================================================================
const noverified = document.getElementById('noverified');
const verified = document.getElementById('verified');
const suspension = document.getElementById('suspension');
const MemberPhoto = document.getElementById('MemberPhoto');
//==========btn===============================================================================================
const btnMemberphoto = document.getElementById('btnphoto');
const btnInsert = document.getElementById('btnInsert');
const btnAlter = document.getElementById('btnAlter');
const btnSubmit = document.getElementById('btnSubmit');
const btnCancel = document.getElementById('btnCancel');
const btnDelete = document.getElementById('btnDelete');
// ==============================================================================================
let lastbtnclickmember = null;
// ==============================================================================================

function Memberreset() {
    //============================================================================
    const MemberId = document.getElementById('MemberId');
    const MemberName = document.getElementById('MemberName');
    const Phone = document.getElementById('Phone');
    const Email = document.getElementById('Email');
    const MemberPassword = document.getElementById('Password');
    const sexm = document.getElementById('sexm');
    const sexfm = document.getElementById('sexfm');
    const Birthday = document.getElementById('Birthday');
    const MemberPhoto = document.getElementById('MemberPhoto');
    const selectcities = document.querySelectorAll('.selectcities');
    const selectlevels = document.querySelectorAll('.selectlevels');
    const noverified = document.getElementById('noverified');
    const verified = document.getElementById('verified');
    const suspension = document.getElementById('suspension');
    const MemberKeyword = document.getElementById('Keyword');

    const btnMemberphoto = document.getElementById('btnphoto');
    const btnAlter = document.getElementById('btnAlter');
    const btnSubmit = document.getElementById('btnSubmit');
    const btnDelete = document.getElementById('btnDelete');
    //===========================================================================
    MemberId.value = '';
    MemberName.value = '';
    Phone.value = '';
    Email.value = '';
    MemberPassword.value = '';
    sexm.checked = true;
    Birthday.value = '';
    MemberPhoto.src = '/images/Member/Noimage.png';
    btnMemberphoto.value = '';
    MemberKeyword.value = '';
    noverified.checked = true;

    selectcities.forEach(op => {
        if (op.value == 1) {
            op.selected = true;
        }
    })
   
    selectlevels.forEach(op => {
        if (op.value == 1) {
            op.selected = true;
        }
    })

    //=============================================================================================
    MemberName.disabled = true;
    Phone.disabled = true;
    Email.disabled = true;
    MemberPassword.disabled = true;
    sexm.disabled = true;
    sexfm.disabled = true;
    Birthday.disabled = true;
    selectareas.disabled = true;
    selectlevel.disabled = true;
    noverified.disabled = true;
    verified.disabled = true;
    suspension.disabled = true;
    btnMemberphoto.disabled = true;
    btnAlter.disabled = true;
    btnSubmit.disabled = true;
    btnDelete.disabled = true;
}
function Memberenable() {
    const MemberName = document.getElementById('MemberName');
    const Phone = document.getElementById('Phone');
    const Email = document.getElementById('Email');
    const MemberPassword = document.getElementById('Password');
    const sexm = document.getElementById('sexm');
    const sexfm = document.getElementById('sexfm');
    const Birthday = document.getElementById('Birthday');
    const selectareas = document.getElementById('selectareas');
    const selectlevel = document.getElementById('selectlevel');
    const noverified = document.getElementById('noverified');
    const verified = document.getElementById('verified');
    const suspension = document.getElementById('suspension');
    const btnMemberphoto = document.getElementById('btnphoto');
    const btnSubmit = document.getElementById('btnSubmit');

    //============================================================================
    MemberName.disabled = false;
    Phone.disabled = false;
    Email.disabled = false;
    MemberPassword.disabled = false;
    sexm.disabled = false;
    sexfm.disabled = false;
    Birthday.disabled = false;
    selectareas.disabled = false;
    selectlevel.disabled = false;
    noverified.disabled = false;
    verified.disabled = false;
    suspension.disabled = false;
    btnMemberphoto.disabled = false;
    btnSubmit.disabled = false;
}
 async function addSelectitems()
{

    const selectareas = document.getElementById('selectareas');
    const selectlevel = document.getElementById('selectlevel');


    const responsecities = await fetch(`https://localhost:7146/MemberApi/GetAreaList`,
        {
            method: "Get"
        })
    const citylist = await responsecities.json();
    const elecitylist = citylist.map(city => {
        return (`<option class="selectcities" value="${city['cityId']}">${city['city1']}</option>`)
    })
    selectareas.innerHTML = elecitylist.join("");
    //=================================================================================================
    const responselevels = await fetch(`https://localhost:7146/MemberApi/GetLevelList`,
        {
            method: "Get"
        })
    const levellist = await responselevels.json();
    const elelevellist = levellist.map(level => {
        return (`<option class="selectlevels" value="${level['memberLevelId']}">${level['memberLevelName']}</option>`)
    })
    selectlevel.innerHTML = elelevellist.join("");
}

//增加點擊事件

function addrowEvent() {
    const allrows = document.querySelectorAll('.newrow');
    const MemberId = document.getElementById('MemberId');
    const MemberName = document.getElementById('MemberName');
    const Phone = document.getElementById('Phone');
    const Email = document.getElementById('Email');
    const MemberPassword = document.getElementById('Password');
    const sexm = document.getElementById('sexm');
    const sexfm = document.getElementById('sexfm');
    const Birthday = document.getElementById('Birthday');
    const MemberPhoto = document.getElementById('MemberPhoto');
    const selectcities = document.querySelectorAll('.selectcities');
    const selectlevels = document.querySelectorAll('.selectlevels');
    const noverified = document.getElementById('noverified');
    const verified = document.getElementById('verified');
    const suspension = document.getElementById('suspension');

    const btnAlter = document.getElementById('btnAlter');
    const btnDelete = document.getElementById('btnDelete');


    allrows.forEach((row) => {
        row.addEventListener('click', async () => {
            let memberId = row.getAttribute('data-memberid');
            const response = await fetch(`https://localhost:7146/MemberApi/GetMemberData/?memberId=${memberId}`,
                {
                    method: "Get"
                })
            const data = await response.json();
            console.log(data);

            MemberId.value = data.memberId;
            MemberName.value = data.memberName;
            Phone.value = data.phone;
            Email.value = data.email;
            MemberPassword.value = data.password;
            sexm.checked = data.gender ? true : false;
            sexfm.checked = data.gender ? false : true;
            Birthday.value = data.birthday.split('T')[0];

            if (data.imagePath != null) {
                MemberPhoto.src = `/images/Member/${data.imagePath}`;
            }
            else {
                MemberPhoto.src = '/images/Member/Noimage.png';
            }
            const selectcities = document.querySelectorAll('.selectcities');
            selectcities.forEach(op => {
                if (op.value == data.cityId) {
                    op.selected = true;
                }
            })
            const selectlevels = document.querySelectorAll('.selectlevels');
            selectlevels.forEach(op => {
                if (op.value == data.memberLevelId) {
                    op.selected = true;
                }
            })
            if (data.memberStatusId == 1) {
                noverified.checked = true;
            }
            else if (data.memberStatusId == 2) {
                verified.checked = true;
            }
            else if (data.memberStatusId == 3) {
                suspension.checked = true;
            }

            window.scrollTo({ top: 0, behavior: 'smooth' });
            //=====改變按鈕=================================================
            btnAlter.disabled = false;
            btnDelete.disabled = false;
        })


    })
}