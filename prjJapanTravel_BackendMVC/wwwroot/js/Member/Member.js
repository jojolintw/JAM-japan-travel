
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

    const selectareas = document.getElementById('selectareas');
    const selectlevel = document.getElementById('selectlevel');

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
    Birthday.value = '1990-01-01';
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
            Memberreset();
            let memberId = row.getAttribute('data-memberid');
            const response = await fetch(`https://localhost:7146/MemberApi/GetMemberData/?memberId=${memberId}`,
                {
                    method: "Get"
                })
            const data = await response.json();

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
//按鈕點擊事件
function addMemberbtnEvent() {
    const memberform = document.getElementById('memberform');
    const membertable = document.getElementById('membertable');

    const MemberId = document.getElementById('MemberId');
    const btnMemberphoto = document.getElementById('btnphoto');
    const btnInsert = document.getElementById('btnInsert');
    const btnAlter = document.getElementById('btnAlter');
    const btnSubmit = document.getElementById('btnSubmit');
    const btnCancel = document.getElementById('btnCancel');
    const btnDelete = document.getElementById('btnDelete');
    let lastmemberbtnclick = '';
    //新增按鈕
    btnInsert.addEventListener('click', (event) => {
        lastmemberbtnclick = event.target;
        Memberreset();
        Memberenable();
        btnInsert.disabled = true;
    })
    //修改按鈕
    btnAlter.addEventListener('click', (event) => {
        lastmemberbtnclick = event.target;
        Memberenable();
    })
    //
    btnDelete.addEventListener('click', async () => {
        const memberId = MemberId.value;
        if (confirm('刪除確認')) {
            const response = await fetch(`https://localhost:7146/MemberApi/DeleteMember/${memberId}`,
                {
                    method: "Get",
                })
            if (response.ok) {
                alert('資料已刪除');

                const allmemberdatas = await response.json();
                const eleallmembers = allmemberdatas.map(mem => {
                    const birthday = new Date(mem.生日).toLocaleDateString('zh-TW', { year: 'numeric', month: 'long', day: 'numeric' });
                    return (`<tr class="newrow" data-memberid="${mem.會員編號}">
                                                <td>
                                                    <p><img src="/images/Member/${mem.頭像路徑}" style="height:100px;width:100px" onerror="this.onerror=null; this.src='/images/Member/Noimage.png';"/></p>
                                                </td>
                                                <td>
                                                    <p>${mem.會員姓名}</p>
                                                </td>
                                                <td>
                                                    <p>${(mem.性別 ? "男" : "女")}</p>
                                                </td>
                                                <td>
                                                    <p>${birthday}</p>
                                                </td>
                                                <td>
                                                    <p>${mem.城市}</p>
                                                </td>
                                                <td>
                                                    <p>${mem.手機號碼}</p>
                                                </td>
                                                <td>
                                                    <p>${mem.email}</p>
                                                </td>
                                                <td>
                                                    <p>${mem.會員等級}</p>
                                                </td>
                                                <td>
                                                    <p>${mem.會員狀態}</p>
                                                </td>
                                            </tr>`)

                })
                membertable.innerHTML = eleallmembers.join("");
                addrowEvent();
            }
            else {
                alert('刪除失敗');
            }
            Memberreset();
        }
    })

    // Submit 按鈕
    btnSubmit.addEventListener('click', async () => {
        console.log(lastmemberbtnclick);

        let Memberform = new FormData(memberform);
        ////=============Insert=====================================================================
        if (lastmemberbtnclick.id == 'btnInsert') {
            const Insertresponse = await fetch(`https://localhost:7146/MemberApi/InsertMember`,
                {
                    method: "POST",
                    body: Memberform,
                })
            const allmemberdatas = await Insertresponse.json();
            const eleallmembers = allmemberdatas.map(mem => {
                const birthday = new Date(mem.生日).toLocaleDateString('zh-TW', { year: 'numeric', month: 'long', day: 'numeric' });
                return (`<tr class="newrow" data-memberid="${mem.會員編號}">
                                    <td>
                                        <p><img src="/images/Member/${mem.頭像路徑}" style="height:100px;width:100px" onerror="this.onerror=null; this.src='/images/Member/Noimage.png';"/></p>
                                    </td>
                                    <td>
                                        <p>${mem.會員姓名}</p>
                                    </td>
                                    <td>
                                        <p>${(mem.性別 ? "男" : "女")}</p>
                                    </td>
                                    <td>
                                        <p>${birthday}</p>
                                    </td>
                                    <td>
                                        <p>${mem.城市}</p>
                                    </td>
                                    <td>
                                        <p>${mem.手機號碼}</p>
                                    </td>
                                    <td>
                                        <p>${mem.email}</p>
                                    </td>
                                    <td>
                                        <p>${mem.會員等級}</p>
                                    </td>
                                    <td>
                                        <p>${mem.會員狀態}</p>
                                    </td>
                                </tr>`)
            })
            membertable.innerHTML = eleallmembers.join("");
            addrowEvent();
        }
        //==============修改Submit===============================================================================
        else if (lastmemberbtnclick.id == 'btnAlter')
        {
            const Alterresponse = await fetch(`https://localhost:7146/MemberApi/AlterMember`,
                {
                    method: "POST",
                    body: Memberform,
                })
            const allmemberdatas = await Alterresponse.json();
            const eleallmembers = allmemberdatas.map(mem => {
                const birthday = new Date(mem.生日).toLocaleDateString('zh-TW', { year: 'numeric', month: 'long', day: 'numeric' });
                return (`<tr class="newrow" data-memberid="${mem.會員編號}">
                                        <td>
                                            <p><img src="/images/Member/${mem.頭像路徑}" style="height:100px;width:100px" onerror="this.onerror=null; this.src='/images/Member/Noimage.png';"/></p>
                                        </td>
                                        <td>
                                            <p>${mem.會員姓名}</p>
                                        </td>
                                        <td>
                                            <p>${(mem.性別 ? "男" : "女")}</p>
                                        </td>
                                        <td>
                                            <p>${birthday}</p>
                                        </td>
                                        <td>
                                            <p>${mem.城市}</p>
                                        </td>
                                        <td>
                                            <p>${mem.手機號碼}</p>
                                        </td>
                                        <td>
                                            <p>${mem.email}</p>
                                        </td>
                                        <td>
                                            <p>${mem.會員等級}</p>
                                        </td>
                                        <td>
                                            <p>${mem.會員狀態}</p>
                                        </td>
                                    </tr>`)

            })
            membertable.innerHTML = eleallmembers.join("");
            addrowEvent();
        }
        Memberreset();
    })
    //取消按鈕
    btnCancel.addEventListener('click', () => {
        Memberreset();
    })
}

function addMemberpicSearch()
{
    const MemberKeyword = document.getElementById('Keyword');
    const btnMemberphoto = document.getElementById('btnphoto');
    const MemberPhoto = document.getElementById('MemberPhoto');
    const btnSubmit = document.getElementById('btnSubmit');
    const membertable = document.getElementById('membertable');
    //=======圖片預覽=================================================
    btnMemberphoto.addEventListener('change', async (event) => {
        const file = event.target.files[0];
        const allowtype = 'image.*';
        if (file.type.match(allowtype)) {
            const reader = new FileReader();
            reader.onload = function () {
                let dataURL = reader.result;
                MemberPhoto.src = `${dataURL}`;
            }
            reader.readAsDataURL(file);
            btnSubmit.disabled = false;
        }
        else {
            alert('圖片格式錯誤');
            btnSubmit.disabled = true;
        }
    })
    //==========關鍵字搜尋====================================
    MemberKeyword.addEventListener('keydown', async (event) => {
        const Keyword = MemberKeyword.value;
        if (event.keyCode == 13 && Keyword.value != '') {
            const response = await fetch(`https://localhost:7146/MemberApi/Search/?keyword=${Keyword}`, {
                method: "Get",
            })
            if (response.ok) {
                const allmembers = await response.json();
                const eleallmembers = allmembers.map(mem => {
                    const birthday = new Date(mem.生日).toLocaleDateString('zh-TW', { year: 'numeric', month: 'long', day: 'numeric' });
                    return (`<tr class="newrow" data-memberid="${mem.會員編號}">
                                                        <td>
                                                            <p><img src="/images/Member/${mem.頭像路徑}" style="height:100px;width:100px" onerror="this.onerror=null; this.src='/images/Member/Noimage.png';"/></p>
                                                        </td>
                                                        <td>
                                                            <p>${mem.會員姓名}</p>
                                                        </td>
                                                        <td>
                                                            <p>${(mem.性別 ? "男" : "女")}</p>
                                                        </td>
                                                        <td>
                                                            <p>${birthday}</p>
                                                        </td>
                                                        <td>
                                                            <p>${mem.城市}</p>
                                                        </td>
                                                        <td>
                                                            <p>${mem.手機號碼}</p>
                                                        </td>
                                                        <td>
                                                            <p>${mem.email}</p>
                                                        </td>
                                                        <td>
                                                            <p>${mem.會員等級}</p>
                                                        </td>
                                                        <td>
                                                            <p>${mem.會員狀態}</p>
                                                        </td>
                                                    </tr>`)

                })
                membertable.innerHTML = eleallmembers.join("");
                addrowEvent();
            }
        }
    })



}