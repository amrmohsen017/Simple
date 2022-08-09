

//the greatest parent that contains kanban cols. in the kanaban template
let kanban_container = $(".kanban-container");
//issue >> this early loading solves the dragging pb. but still doesn't solve the events hooking that you need on already init. draggables :)
//kanban_container.append(kanban_column_getter({"status_id":2 , status_name:"ahmed"}))

let new_task = $(".new_task");

//1. connection is created  implicitly via the dynamic proxy :) 
$.connection.hub.logging = true;
var taskProxy = $.connection.task;



// senario#1 :: client sends data to server to process and then respond back
console.log(taskProxy.client)


taskProxy.client.clientMethod = function (msg) {
    console.log('Server sent: ' + msg);
};


taskProxy.client.sessionMethod = function (date) {
    console.log('User sessions Count: ' + date);
};


// scenario#3 :: getting data from hubContext inside an mvc controller :)
taskProxy.client.controllerMethod = function (date) {
    //alert(date)
    console.log(date);
};
// senario#3 END 



function kanban_column_getter(status) {

    let stub = `
<div  class="kanban-column">

        <div class="kanban-column-header">
            <h5 class="fs-0 mb-0">${status.status_name} 
         
        </div>


        <div class="kanban-items-container scrollbar" status_id="${status.status_id}">
            



        
    </div>


`

    return stub



}


function task_card_getter(task) {


    let desc_exist = task.task_description ? `<p class="mb-0 fw-medium font-sans-serif stretched-link" >Task description: <br> ${task.task_description}</p>
  ` : "";
    let dates_exist = task.task_planned_start && task.task_planned_end ? `
    <nav style="--falcon-breadcrumb-divider: '»';" aria-label="breadcrumb">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item">${new Date(task.task_planned_start).toLocaleDateString("en-US")}</li>
                            <li class="breadcrumb-item active" aria-current="page">${new Date(task.task_planned_end).toLocaleDateString("en-US")}</li>
                        </ol>
                    </nav> ` : "";

    let task_card = `
<div class="kanban-item"   >
 


 <div class="dropdown font-sans-serif btn-reveal-trigger">
                    <button class="btn btn-sm btn-reveal my-1 py-0 px-2" type="button" id="kanbanColumn1" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                    
                    <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="kanbanColumn1">
                        <a class="dropdown-item" href="Edit/${task.task_id}"  id="edit_task">Edit</a>
                        <div class="dropdown-divider"></div><a class="dropdown-item text-danger" data-task_id=${task.task_id} data-sub_task_id=${task.sub_task} onclick="delete_task(this.getAttribute('data-task_id') , this.getAttribute('data-sub_task_id') , false )" id="delete_task">Remove</a>
                    </div>

                </div>





            <div class="card kanban-item-card" data-bs-toggle="modal" data-bs-target="#task_detail" id="task_id_${task.task_id}" data-task_id=${task.task_id} data-sub_task_id=${task.sub_task}>
                <div class="card-body">


                 
                    <h5> ${task.task_name} </h5>
                    <hr>
               
                ${desc_exist}

                 ${dates_exist}


                


                </div>
            </div>
        </div>
`
  
    return task_card; 
}



//console.log(document.getElementById('test_drag'))




let statuses = null; 
$.connection.hub.start(


    function () {

      

        taskProxy.server.server_get_tasks().then(tasks_and_statuses => {


           
            tasks_and_statuses_js = JSON.parse(tasks_and_statuses)
           
            tasks = tasks_and_statuses_js.tasks
            statuses = tasks_and_statuses_js.statuses
            console.log(statuses)
            statuses.forEach(s => {
              
                kanban_container.append(kanban_column_getter(s))


            })

            //ahmed
            //const draggable = new Draggable.Draggable(document.getElementById('test_drag'), {
            //    draggable:'.kanban-item'
            //});

            //draggable.on('drag:move', () => console.log('drag:start'));

            console.log(tasks)
           



            tasks.forEach(task => {


                // i will define a main task as (has a subtask & doesn't have a supervisor)
                // I could show sub-tasks in a card view 
                if ( task.task_supervisor) {
                  
                    if (task.status_id) {

                        $(`div[status_id="${task.status_id}"]`).append(task_card_getter(task))

                    } else {
                        new_task.append(task_card_getter(task))
                    }
       
                    //$(`div[status_id="${task.status_id}"]`).append(task_card_getter(task))
                   
                }
              
              
              

            });

        }
            
        
            
            )

        //$(".kanban-item").draggable({
        //    start: function () {
        //        console.log("YEAHH")
        //    },
        //    drag: function () {
        //        console.log("YEAHH")
        //    },
        //    stop: function () {

        //    }
        //});

        console.log("connection started!");


        if ($(location).attr('pathname').includes("Tasks/Edit")) {


            taskId = $(".modal-body").attr('id');
            
           

            subtasks_ajax()

            console.log("selected ", selected)

            taskProxy.server.server_get_task_details(parseInt(taskId)).then((task_details) => {


                //console.log(task_details)
            
                task_details = JSON.parse(task_details)
                task_details_global = task_details //just don't forget if any shallow copying is done here :)
                let modelBody_title = $(".task_modal_body h4")
          
                let status_select = $("#status")
                let task_desc = $(".task_desc")
                task_desc.html(task_details.task[0].task_description)
              


                $("#save_task_ingreds").attr("save_task_ingreds", parseInt(task_details.task[0].task_id));

                let supervised_by = $("#supervised_by")
                supervised_by.html(task_details.task[0].username)



                taskProxy.server.get_task_logs(parseInt(task_details.task[0].task_id)).then((task_log) => {
                    task_logs.empty()


                    task_log.forEach(log => {

                        task_logs.append(`<li> ${log}</li>`)

                    });


                })




                status_select.empty()

                statusId = task_details.task[0].status_id; 
                status_select.append(`<option value="" ${task_details.task[0].status_id ? 'selected' : ''}>Select a status</option>`)
                task_details.statuses.forEach(s => {

                    
                    status_select.append(`<option value=${s.status_id} ${task_details.task[0].status_id == s.status_id ? 'selected' : ''} >${s.status_name}</option>`)
                   

                });


       


                modelBody_title.text(task_details.task[0].task_name);


                populate_task_selects(task_details, document.getElementById("assignees"), "assignees")
                populate_task_selects(task_details, document.getElementById("tags"), "tags")
                //populate_task_selects(task_details, document.getElementById("status"), "status")





            })
        }


        //desc editor 
        //tinymce.init({
        //    selector: 'textarea#editor'
        //})

      


        // scenario#2 :: invoke a method on server and get its output immediately "useful when getting data from the hubcontext :)       
        //taskProxy.server.serverMethod2().then(data => console.log(data))
        // senario#2 END 

    }

).done(function () {


    //taskProxy.server.serverMethod("client value");
    console.log(taskProxy.server)
    // methods signatures should match friend :) 
    //taskProxy.server.serverMethod();

});





//client side validation 

let toastEl = $("#client_side_validation")
let toastLiveExample = document.getElementById('liveToast')
let task_logs = $("#task_logs")




//taskProxy.client.client_get_tasks = function (tasks) {
    
//    let new_task = $(".new_task"); 
//    tasks.forEach(task => {
       
//        new_task.append(task_card_getter(task))

//    });
//};



function intercept_create_task() {

    let is_valid = task_name.value != ""

   
    toastEl.text('')

    if (!is_valid) {
      
        toastLiveExample.className = "toast bg-danger";
       
        
        toastEl.append(`<li>Task name is required </li>`)
        new bootstrap.Toast(toastLiveExample).show()
        return false; 

    } 
    
  

    return true;


}





taskProxy.client.client_get_task = function (task , error = false , errors = null , edited = false , sub_task = null) {

    

    toastEl.text('')

    if (error) {

        toastLiveExample.className = "toast bg-danger";


        errors.forEach(e => {


            toastEl.append(`<li>${e}</li>`)

        });

        new bootstrap.Toast(toastLiveExample).show()

        return

    } else if (edited && sub_task) {

        task_logs.append(`<li>Sub task updated</li>`)

        toastLiveExample.className = "toast bg-success";
        console.log("LOVE YA" , task)
        $(`#task_id_${task.task_id} h5`).html(task.sub_task_name)

        toastEl.append(`<h4>Sub task edited successfully </h4>`)
        new bootstrap.Toast(toastLiveExample).show()

        subtasks_table.ajax.reload();

    }


    else if (edited) {
       
        if (task) {
            task_status_changed(task);
        }
       

        task_logs.append(`<li>Task updated</li>`)

        toastLiveExample.className = "toast bg-success";

        toastEl.append(`<h4>Task edited successfully </h4>`)
        new bootstrap.Toast(toastLiveExample).show()
   


    }

    

    else if (sub_task) {
        
        toastLiveExample.className = "toast bg-success";

       
        toastEl.append(`<h4>A new sub_task is created</h4>`)

      
       
        $(`div[data-task_id="${taskId}"]`).attr('data-sub_task_id', task.task_id ); 
  
        // I should NOT show the sub-tasks in a card view though :)
        //$(".new_task").append(task_card_getter(task))

        new bootstrap.Toast(toastLiveExample).show()

        subtasks_table.ajax.reload();

    }

    else {
        toastLiveExample.className = "toast bg-success";

        toastEl.append(`<h4>A new task created</h4>`)
        new bootstrap.Toast(toastLiveExample).show()

        
        $(".new_task").append(task_card_getter(task))
    }

   

   
};







let selected_from_db = {


    "assignees": [],
    "tags": [],
    "status": null,
    "assignees_new": []


}
let selected = {

   
    "assignees": [],
    "tags": [],
    "status": null,
    "assignees_new": []
   

}


let choices_cache = {}

function removeItemInPlace(array, item) {
    let i
    while ((i = array.indexOf(item, i)) > -1) {
        array.splice(i, 1)
    }
    return array.length
}


function register_select_events(choices, select_data ) {


    /*

select_data =[ select_from_db , staging_select]
both are lists of ids 

<pt> i only show choices that are not in select_from_db ...

when adding an item :: 
due to <pt> i only add what is not in select_from_db so it's not found in select_from_db 
so the only option >> if it's found in staging_select ? don't add it 

when removing an item :: 
if found in select_from_db ? delete in place 
else found in staging_select ? delete in place

    */
    
        choices.passedElement.element.addEventListener(
            'addItem',
            function (event) {
              
                console.log("IN add item before: ", selected, event.detail)
            

                 select_data.push( event.detail.value)
               
                  


                console.log("IN add item AFTTERRR: ", selected, event.detail)



            },
            false,
        );
      
        choices.passedElement.element.addEventListener(
            'removeItem',
            function (event) {
                console.log("IN REMOVE ITEM beforee: ", selected, event.detail)


                removeItemInPlace(select_data, event.detail.value)

                console.log("IN REMOVE ITEM after: ", selected, event.detail)

          


            },
            false,
        );

   

}


let staging_select = {}
function populate_task_selects(task_details, select_el , id) {
    let mappings = null; 
    let choices = null; 

    //populate selected from db HERE MAN ! 


    selected_from_db.assignees = [...task_details.task_assignees]
    selected_from_db.tags = [...task_details.task_tags]

  
  
    console.log("Always When i click a card: ", selected)
   
    if (!(id in choices_cache) || id == "assignees_new" ) {

        
        choices = new Choices(select_el,

            { removeItemButton: true })


      

        choices_cache[id] = choices;

        console.log("Before register: selected ", selected)
        register_select_events(choices, selected[id]

        

        )

  

    } else {
        choices = choices_cache[id]
    }


    console.log("laugh::" , id , choices)
    choices.removeActiveItems();

   

    if (id == "tags") {
        choices.setValue(selected_from_db.tags.map(t => {
            let obj = {}
            obj.value = t.tag_id;
            obj.label = t.tagname;
            obj.id = t.tag_id;
            return obj
        }
            ));


    } else if (id == "assignees") {
        
        choices.setValue(selected_from_db.assignees.map(t => {
            let obj = {}
            obj.value = t.user_id;
            obj.label = t.username;
            obj.id = t.user_id;
        
            return obj



        }));
    }

    
  
    switch (id) {

       
       
        case "assignees":
        
            mappings = task_details.users.filter(function (ele) {
                return !selected_from_db["assignees"].map(t => t.user_id).includes(ele.user_id)
            }).map((user) => {

                let obj = {}
                obj.value = user.user_id;
                obj.label = user.username;
                obj.id = user.user_id;
                return obj


            })

     

            break; 
        case "tags":

            mappings = task_details.tags.filter(function (ele) {
                return !selected_from_db["tags"].map(t => t.tag_id).includes(ele.tag_id)
            }).map((tag) => {

                let obj = {}
                obj.value = tag.tag_id;
                obj.label = tag.tagname;
                obj.id = tag.tag_id;
                return obj


            })

            break; 

        case "status":
       

            mappings = task_details.statuses.filter(function (ele) {
                return !selected_from_db[id].includes(ele.status_id)
            }).map((s) => {

                let obj = {}
                obj.value = s.status_id;
                obj.label = s.status_name;
                obj.id = s.status_id;
                return obj


            })


   
            break; 

    }


    choices.clearChoices();
 
    choices.setChoices(
        mappings,
        'value',
        'label',
        false,
    );

    
    


}


// Crucial thinking :) :) :)
// as i need to reload the ajax data for specific subtask after click 
// and i need to only init datatable once
// and i post this dynamic taskId on every reload 
// i will populate taskId after each click , and then once reload it should be correct :)
// also note that i deferLoading: 0 to stop the init datatable ajax request as taskId would be null by then :)

let taskId = null; 
let statusId = null; 
let sub_taskId = null; 


// by the same token i will define a task_details global 

let task_details_global = null; 

$(document).on("click", ".kanban-item-card", function (e) {

    console.log("IS THAT YOUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU")
 


 
    taskId = $(this).data('task_id');
    sub_taskId = $(this).attr('data-sub_task_id');

    statusId = $("#status");


    subtasks_ajax()

    console.log("selected " ,  selected)

    taskProxy.server.server_get_task_details(parseInt(taskId)).then((task_details) => {

      
     
     
        task_details = JSON.parse(task_details)
        task_details_global = task_details //just don't forget if any shallow copying is done here :)
        let modelBody_title = $(".task_modal_body h4")
    
        let status_select = $("#status")
        let task_desc = $(".task_desc")
        task_desc.html(task_details.task[0].task_description)
      
       

        $("#save_task_ingreds").attr("save_task_ingreds", parseInt(task_details.task[0].task_id));

        let supervised_by = $("#supervised_by")
        supervised_by.html(task_details.task[0].username)

      
   
        taskProxy.server.get_task_logs(parseInt(task_details.task[0].task_id)).then((task_log) => {
            task_logs.empty()

            
            task_log.forEach(log => {

                task_logs.append(`<li> ${log}</li>`)

            });
           

        })

        


        status_select.empty()

        statusId = task_details.task[0].status_id; 
        status_select.append(`<option value="" ${task_details.task[0].status_id ? 'selected' : ''}>Select a status</option>`)
        task_details.statuses.forEach(s => {


            status_select.append(`<option value=${s.status_id} ${task_details.task[0].status_id == s.status_id ? 'selected' : ''} >${s.status_name}</option>`)

        });

       


     
       
      
        modelBody_title.text(task_details.task[0].task_name);


    
        populate_task_selects(task_details, document.getElementById("assignees"), "assignees")
        populate_task_selects(task_details, document.getElementById("tags"), "tags")
        //populate_task_selects(task_details, document.getElementById("status"), "status")


      


    })
 
});


function task_status_changed(task) {

    $(`#task_id_${task.task_id}`).parent().remove();
    $(`div[status_id="${task.status_id}"]`).append(task_card_getter(task))


}


// when i call this method ? 
// when i delete a main task card >> taskID needed
// when i delete a subtask from within a main task card >> taskID && subtaskid needed
function delete_task( task_id , sub_task_id, delete_sub_task) {

    //console.log(task_id)
    //return;
    
    if ( task_id == 0) {
        return; 
    }


    if (!task_id ) {
        task_id = 0;
    }

    if (delete_sub_task) {
        console.log("Deleting a sub-task(and possibly its parent task )with id " + sub_task_id, typeof (sub_task_id))
        return taskProxy.server.server_delete_task(task_id ? parseInt(task_id) : 0, sub_task_id , true).then((done) => {



            //i can show a sub-task as a card though :)
            //$(`#task_id_${sub_task_id}`).parent().remove();
         
            toastLiveExample.className = "toast bg-danger";

            toastEl.text('')
            toastEl.append(`<li>Sub-tasks deleted successfully</li>`)
            new bootstrap.Toast(toastLiveExample).show()

            subtasks_table.ajax.reload();

        })

    } else {
        console.log("Deleting a task with id " + task_id)
        return taskProxy.server.server_delete_task(parseInt(task_id), 0 , false).then((done) => {


            $(`#task_id_${task_id}`).parent().remove();

            toastLiveExample.className = "toast bg-danger";

            toastEl.text('')
            toastEl.append(`<li>Task deleted successfully</li>`)
            new bootstrap.Toast(toastLiveExample).show()

            //subtasks_table.ajax.reload();

        })

    }

  


}





//taskProxy.client.task_edited = function () {
   
//    task_logs.append(`<li>Task updated</li>`)

//}





$("#save_task_ingreds").on("click", function (e) {
    console.log("test", selected)


    //return 
    selected.task_id = $("#save_task_ingreds").attr('save_task_ingreds')
    selected.status = $("#status").val()


    //A little change detetion system :)
    let tags_filter_length = selected.tags.filter(t => !selected_from_db["tags"].map(t => t.tag_id).includes(t)).length
    let assignees_filter_length = selected.assignees.filter(t => !selected_from_db["assignees"].map(t => t.user_id).includes(t)).length

    //console.log("INGREDS", selected)
    //console.log("INGREDS", selected_from_db)
    //console.log("INGREDS", tags_filter_length, assignees_filter_length)


    if (tags_filter_length || assignees_filter_length || selected.assignees.length != selected_from_db.assignees.length || selected.tags.length != selected_from_db.tags.length || ($("#status").val() && $("#status").val() != statusId)) {
        taskProxy.server.update_task_details(JSON.stringify(selected), null, null, false).then(data => {

            //reset the selected to not save same values again which is not necessary :)
            // but a HUGE gutcha :) u don't reset the reference BUT the data :) , so the following line is wrong 
            //selected = {


            //    "assignees": [],
            //    "tags": [],
            //    "status": [],
            //    "assignees_new": []


            //}
            selected["assignees"].length = 0
            selected["tags"].length = 0
            selected["status"].length = 0
          
      
            //i should also re-populate the selected_from_db but i will force close the modal though :)
            $('#close_modal').click();

        }


        )
    } else {
        let toastEl = $("#client_side_validation")
        toastLiveExample.className = "toast bg-danger";

        toastEl.text('')
        toastEl.append(`<li>No changes to save</li>`)
        new bootstrap.Toast(toastLiveExample).show()

    }
  


})




// subtasks LOGIC 



var subtasks_table =  $("#subtasks").DataTable({
    autoWidth: false,
    deferLoading : 0,
    serverSide: true,
    deferRender: true,
 
    scrollCollapse: true,
    scroller: true,
    ajax: {
        url: "/test",

        
        type: 'POST',


        dataSrc: function (data) {

        
            if (data.recordsTotal == 0) {

          
                data.data = [{
                    //"$id": "1",
                    "task_id": 0,
                    "task_name": "",
                    "task_description": null,
                    "task_planned_start": null,
                    "task_planned_end": null,
                    "task_deadline": null
                 
                }]
            
            }

           
            
            return data.data
        },
        data: function (data) {
            data.task_id = taskId
            console.log(data)
            return data
        }

    },

    columnDefs: [
        {
        targets : 0,
        searchable : true
        },
        {
            targets: 1,
            searchable: true
        },
        {
            targets: 2,
            searchable: false,
            orderable: false

        },


    ]
    ,
    columns: [

        {
            data: "task_name", class: 'editable text',
            
            render: function (data, type, row) {
               
         

                   return `<a class="link-primary" href="/Tasks/Edit/${row.task_id}"> ${data} </a>`
            }
        },
        
        {
            data: "task_deadline", class: 'editable datepicker', width: "40%",
            render: function (data) {
              

                return data ? new Date(data).toLocaleDateString("en-US") : data
            }


        },
        
        {
            data: "task_id",
            render: function (task_id ,type,  row) {

                //ahmed 
                console.log("welcome: ", sub_taskId)
            /*  let temp = `<div><a class='edit' style="cursor: pointer;" > Edit </a> <br>`*/
                let temp = "" ; 
                if (sub_taskId == task_id) {
                    temp += `

<div><a class='edit' style="cursor: pointer;" > Edit </a> <br>
<a  style="cursor: pointer;" class='delete' parent_task_id="${taskId}" id ="${task_id}" > Delete </a> </div>
`
                }
                //else {
                //    temp +=`</div>`
                //}

                if (task_id == 0) {
                    temp = `<div><a class='edit' style="cursor: pointer;" > Edit </a></div>`
                }
                return temp;



                    ;
            }
        },



    ]

}


); 


function subtasks_ajax() {

    subtasks_table.ajax.reload(); 
}


let sub_tasks = $("#subtasks");



sub_tasks.on('click', 'tbody td .add', function (e) {


    //var clickedRow = $($(this).closest('td')).closest('tr');
    let new_row_html = `


<tr>
                            <td class="editable text">



   ${fnCreateTextBox('', 'name')}
    
    
    </td>
             


   <td class="editable"> 
    {2}
     </td>




    <td class="editable datepicker"> 
 <input class="form-control datepicker new_dead_line" data-field="dead_line">  </input>
    </td>


       <td> 
    <div><a class='add' href='#'> Add </a> <br><a class='update' href='#'> Update </a> <br>
                            <a class='delete' href='#'> Delete </a> </div>
       
   
      </td>


                            </tr>

`


    $(this).parents('tbody').append(new_row_html)
    
})

$(document).on('focusin', '.new_dead_line', function () {
    $(this).datepicker({
        dateFormat: "mm/dd/yy",
        changemonth: true,
        changeyear: true
    })

});




sub_tasks.on('click', 'tbody td .edit', function (e) {

    fnResetControls();
    
   

    var clickedRow = $($(this).closest('td')).closest('tr');
    $(clickedRow).find('td').each(function () {
        // do your cool stuff
        if ($(this).hasClass('editable')) {
            if ($(this).hasClass('text')) {
                console.log($(this).html(), $($(this).html()).text())
                var html = fnCreateTextBox($($(this).html()).text(), 'name');
                $(this).html($(html))
            }
            else if ($(this).hasClass('datepicker')) {
                var html = fnCreateDatepicker($(this).html(), 'datepicker');
                $(this).html($(html).datepicker({
                    dateFormat: "mm/dd/yy",
                    changemonth: true,
                    changeyear: true
                }))
            }
            //else if ($(this).hasClass('assignee')) {
            //    //var html = fnCreateDropdown($(this).html(), 'assignee', true);
            //    var html = fnCreateDropdown(true);
            //    $(this).html($(html))

            //    populate_task_selects(task_details_global, document.getElementById("assignees_new"), "assignees_new")

            //}

        }
    });


    $('#subtasks tbody tr td .update').removeClass('update').addClass('edit').html('Edit');
    $('#subtasks tbody tr td .cancel').removeClass('cancel').addClass('delete').html('Delete');

    $(clickedRow).find('td .edit').removeClass('edit').addClass('update').html('Update');
    $(clickedRow).find('td .delete').removeClass('delete').addClass('cancel').html('Cancel');

});

function fnCreateTextBox(value, fieldprop) {

    return `<input class="form-control" data-field=${fieldprop} type="text" value='${value}' >  </input>`;
}
function fnCreateDatepicker(value, fieldprop) {

    return '<input class="form-control datepicker" data-field="' + fieldprop + '" type="text" value="' + value + '" ></input>';
}

function fnCreateDropdown( show) {
    if (show) {
 
       return $("#assignees_new").attr("style", "display:inline")
    }
    return `<select style="display:none"  class="form-select choices-multiple" multiple id="assignees_new" data-options='{"removeItemButton":true,"placeholder":true}'>
    
    </select>
 
    
    `
  

}

sub_tasks.on('click', 'tbody td .cancel', function (e) {
    fnResetControls();
    $('#subtasks tbody tr td .update').removeClass('update').addClass('edit').html('Edit');
    $('#subtasks tbody tr td .cancel').removeClass('cancel').addClass('delete').html('Delete');
});


function fnResetControls() {
  
    var openedTextBox = sub_tasks.find('input');
    $.each(openedTextBox, function (k, $cell) {
        $(openedTextBox[k]).closest('td').html($cell.value);
    })
}


let data_of_subtask = {}
sub_tasks.on('click', 'tbody td .update', function (e) {

    var openedTextBox = sub_tasks.find('input');

    let subTask_cell_to_add_its_dbIdAfterSave = null; 
    let subTask_cell_value = null; 
    $.each(openedTextBox, function (k, $cell) {


        //todo :: task_name must not be empty and then add to db and refresh :)

        if ($($cell).attr('data-field') === "name" && $cell.value == "") {

            let toastEl = $("#client_side_validation")
            toastLiveExample.className = "toast bg-danger";

            toastEl.text('')
            toastEl.append(`<li>Task name is required </li>`)
            new bootstrap.Toast(toastLiveExample).show()

            return false;


        }
        if ($($cell).attr('data-field') === "name") {
            subTask_cell_to_add_its_dbIdAfterSave = $(openedTextBox[k]).closest('td');
            subTask_cell_value = $cell.value;

            subTask_cell_to_add_its_dbIdAfterSave.html(`<a> Saving ${subTask_cell_value}.... </a>`);
        }
        else {
           

            //fnUpdateDataTableValue($cell, $cell.value);
            $(openedTextBox[k]).closest('td').html($cell.value);
        }

        data_of_subtask[$($cell).attr('data-field')] = $cell.value
     

        $('#subtasks tbody tr td .update').removeClass('update').addClass('edit').html('Edit');
        $('#subtasks tbody tr td .cancel').removeClass('cancel').addClass('delete').html('Delete');

        console.log($($cell).attr('data-field'), $cell.value)
    })


    selected.task_id = taskId
    selected.sub_task_id = $(this).next().next().attr('id') 

 
  //ahmed 
    taskProxy.server.update_task_details(JSON.stringify(selected), data_of_subtask.name, data_of_subtask.datepicker, true).then(

      
        sub_task_id => {
 
       
            subTask_cell_to_add_its_dbIdAfterSave.replaceWith(`<td class=" editable text"> <a class="link-primary" href="/Edit/${sub_task_id}"> ${subTask_cell_value} </a> </td>`);



        }

    )
     
  
    //taskProxy.server.server_get_task(taskId, data_of_subtask.name, data_of_subtask.datepicker, null, null, null, 0, false)
    console.log(data_of_subtask)


});

function fnUpdateDataTableValue($inputCell, value) {

    var dataTable = sub_tasks.DataTable();
    var rowIndex = dataTable.row($($inputCell).closest('tr')).index();
    var fieldName = $($inputCell).attr('data-field');
    if (fieldName == "datepicker") {

        dataTable.rows().data()[rowIndex]['task_deadline'] = value
    }
    else {
        console.log(rowIndex, dataTable.rows().data(), dataTable.rows().data()[rowIndex])
        dataTable.rows().data()[rowIndex][fieldName] = value;
    }


}


$("#subtasks").on('click', 'tbody td .delete', function (e) {
    /*  console.log($(this).attr("id"))*/
    //console.log()
    let possible_first_row = $(this).parents('tr');
    if (possible_first_row.parent().prop("tagName").toLowerCase() == "tbody") {
        possible_first_row.html(`

   <td class="editable text"> 

    </td>

   <td class="editable datepicker">

    </td>

 <td>
<div><a class='edit' > Edit </a> <br><a class='delete' id =""> Delete </a> </div>
  </td>

`)

  
         

    }


   
    //delete_task(null, $(this).attr("id"))
    delete_task($(this).attr("parent_task_id"), $(this).attr("id") , true)

    

   


    //subtasks_table.ajax.reload();



}
)