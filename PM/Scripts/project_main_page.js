


$(document).ready(function () {

	var all_stages = @Html.Raw(Json.Encode(Model.stages));
	var all_projects = @Html.Raw(Json.Encode(Model.projects));
	console.log(all_stages);
	var institutes = @Html.Raw(Json.Encode(Model.institutes));
	var inst_choices = institutes.map(t => {
		let obj = {}
		obj.value = t.institute_id;
		obj.label = t.institutename;
		obj.id = t.institute_id;
		return obj
	});
	var choices = new Choices(document.getElementById("institute_id"), { choices: inst_choices, removeItemButton: true });



	var stage_choices = all_stages.map(t => {
		let obj = {}
		obj.value = t.stage_id;
		obj.label = t.stage_name;
		obj.id = t.stage_id;
		return obj
	});
	var s_choices = new Choices(document.getElementById("stage_id"), { choices: stage_choices, removeItemButton: true });


	search();

	//creating item
	//var scratched_item = jQuery('<div>', {
	//	id: 'kanban_item',
	//	class: 'kanban-item'
	//});



	//var div = '<div class="card kanban-item-card hover-actions-trigger">\
	//					<div class="card-body">\
	//						<p class="mb-0 fw-medium font-sans-serif stretched-link" data-bs-toggle="modal" data-bs-target="#project_details" id="project_data" onclick="project_details(this.id)">\
	//							<strong id="project_hint"></strong>\
	//						</p>\
	//					</div>\
	//				  </div>';


	//scratched_item.append(div);





	//var kanbanContainer = $("#kanban_container");
	//$("#kanban_column").attr("style", "display: none !important;")
	//var kanbanItem = $("#kanban_item");
	//var mainKanbanColumn = $("#kanban_column").clone();
	//mainKanbanColumn.attr("style", "display: visible;")

	////adding columns equal to stages we have in DB.
	//$.each(all_stages, function (index, val) {
	//	var kanbanColumn = mainKanbanColumn.clone();
	//	var stage_id = val.stage_id.toString();
	//	kanbanColumn.attr("id", "kanban_column_" + stage_id); //changing the kanban column id.
	//	kanbanColumn.find("#column_scrollbar").attr("id", "column_scrollbar_" + stage_id);
	//	kanbanColumn.find("#add_card").attr("id", "add_card_" + stage_id);
	//	kanbanColumn.find(".stage_name").text(val.stage_name); //changing the name of the column to the stage name.
	//	kanbanContainer.append(kanbanColumn); //adding the columns we created according to the stages we have in DB.
	//});


	////adding projects to columns according to its stages id.
	//$.each(all_projects, function (index, val) {
	//	var project_id = val.project_id.toString();
	//	var project_stage_id = val.project_stage_id.toString();
	//	var item = scratched_item.clone();

	//	item.attr("id", "kanban_item_" + project_id); //changing the kanban item to have the project id
	//	item.find("#project_data").attr("id", "project_data_" + project_id);
	//	item.find("#project_hint").text(val.projectname); //setting the card to its equivalent project name
	//	/*item.find("#project_data_" + project_id).append("<br>");*/
	//	item.append(`<a href="add_more_project_data?project_id=${project_id}">add more data for ${val.projectname}</a>`);
	//	$("#column_scrollbar_" + project_stage_id).append(item); //appending the project card to the equivalent stage.


	//});

	////validation on creating the project.
	//$("#create_project").click(function () {

	//	var projectname = $("#project_name").val();

	//	//validation
	//	const regularExp = new RegExp('^[a-zA-z\u0621-\u064A ]+[a-zA-z0-9\u0621-\u064A\u0660-\u0669]+');

	//	if (!regularExp.test(projectname)) {
	//		alert("لا يمكن ادخال ارقام فقط");
	//		return false;
	//	}


	//})



	//$.each(all_stages, function (index, val) {

	//	$("#stage_dropdown").append("<option value='" + val.stage_id.toString() + "'>" + val.stage_name + "</option>");

	//});





});

	function search() {

	var all_stages = @Html.Raw(Json.Encode(Model.stages_dict));
	var all_projects = @Html.Raw(Json.Encode(Model.projects));
	var search_text = $("#search_input").val();
	$("#kanban_container").empty();
	//var searched_stages = [];
	var searched_projects = [];
	var searched_stages = new Set();
	if (search_text != "") {



		for (const key in all_projects) {
			if (all_projects[key].projectname.includes(search_text)) {

		searched_projects.push(all_projects[key]);

	searched_stages.add(all_stages[all_projects[key].project_stage_id]);
			}

		}


	for (const key in all_stages) {

			if (all_stages[key].stage_name.includes(search_text)) {
		searched_stages.add(all_stages[key]);
			}

		}

	all_stages = Array.from(searched_stages);
	all_projects = searched_projects;
	console.log('projects after', all_projects);
	console.log('stages after', all_stages);

	}


	var scratched_item = jQuery('<div>', {
		id: 'kanban_item',
		class: 'kanban-item'
	});



		var item_div = '<div class="card kanban-item-card hover-actions-trigger">\
			<div class="card-body">\
				<p class="mb-0 fw-medium font-sans-serif stretched-link" data-bs-toggle="modal" data-bs-target="#project_details" id="project_data" onclick="project_details(this.id)">\
					<strong id="project_hint"></strong>\
				</p>\
			</div>\
		</div>';


		scratched_item.append(item_div);

		var scratched_column = jQuery('<div>', {
			id: 'kanban_column',
			class: 'kanban-column'
	});

			var column_div = '<div class="kanban-column-header">\
				<h5 class="fs-0 mb-0 stage_name"><span class="text-500">(8)</span></h5>\
				<div class="dropdown font-sans-serif btn-reveal-trigger">\
					<button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="kanbanColumn0" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>\
					<div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="kanbanColumn0">\
						<a class="dropdown-item" id="add_card">Add Project</a><a class="dropdown-item" href="#!" id="edit_card">Edit</a>\
						<div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!" id="remove_card">Remove</a>\
					</div>\
				</div>\
			</div>\
			<div class="kanban-items-container scrollbar" id="column_scrollbar"></div>';

			var create_proj_div = '<div class="kanban-column-footer"><button class="btn btn-link btn-sm d-block w-100 btn-add-card text-decoration-none text-600" data-bs-toggle="modal" data-bs-target="#add_project_window" type="button"><span class="fas fa-plus me-2"></span>انشاء مشروغ جديد</button></div>'

			scratched_column.append(column_div);



			var kanbanContainer = $("#kanban_container");
			kanbanContainer.append(create_proj_div);
			//$("#kanban_column").attr("style", "display: none !important;")

			//var kanbanItem = $("#kanban_item");
			var mainKanbanColumn = scratched_column.clone();
			//mainKanbanColumn.attr("style", "display: visible;")

			//adding columns equal to stages we have in DB.
			$.each(all_stages, function (index, val) {
		var kanbanColumn = mainKanbanColumn.clone();
			var stage_id = val.stage_id.toString();
			kanbanColumn.attr("id", "kanban_column_" + stage_id); //changing the kanban column id.
			kanbanColumn.find("#column_scrollbar").attr("id", "column_scrollbar_" + stage_id);
			kanbanColumn.find("#add_card").attr("id", "add_card_" + stage_id);
			kanbanColumn.find(".stage_name").text(val.stage_name); //changing the name of the column to the stage name.
			kanbanContainer.append(kanbanColumn); //adding the columns we created according to the stages we have in DB.
	});


			//adding projects to columns according to its stages id.
			$.each(all_projects, function (index, val) {
		var project_id = val.project_id.toString();
			var project_stage_id = val.project_stage_id.toString();
			var item = scratched_item.clone();

			item.attr("id", "kanban_item_" + project_id); //changing the kanban item to have the project id
			item.find("#project_data").attr("id", "project_data_" + project_id);
			item.find("#project_hint").text(val.projectname); //setting the card to its equivalent project name
		/*item.find("#project_data_" + project_id).append("<br>");*/
				item.append(`<a href="add_more_project_data?project_id=${project_id}">add more data for ${val.projectname}</a>`);
				$("#column_scrollbar_" + project_stage_id).append(item); //appending the project card to the equivalent stage.

	});

				$.each(all_stages, function (index, val) {

					$("#stage_dropdown").append("<option value='" + val.stage_id.toString() + "'>" + val.stage_name + "</option>");

	});

}




				//this function for adding a new project
				function addCard(item_id) {
	var card_item = $("#kanban_item");
				var word_list = item_id.split("_");
				var id_number = word_list[word_list.length - 1]; //getting the last char in id string

				$("#column_scrollbar_" + id_number).append(card_item.clone());

}



				function project_details(project_id) {
	var word_list = project_id.split("_");
				var id = word_list[word_list.length - 1];

				$("#project_id").val(id);



				var all_projects = @Html.Raw(Json.Encode(Model.projects));

				var project = all_projects[id];

				console.log('project', project);
				$("#project_name_window").val(project['projectname']);
				$("#project_desc_window").val(project['description']);
				$("#projec_start_date").val(project['plannedstartdate']);
				$("#projec_end_date").val(project['plannedenddate']);
				$("#projec_dead_date").val(project['deadline_date']);
				//$("#institute_name").text(project['projectname']);
				$("#cost_field").val(project['cost']);
				$("#stage_name").val(project['stage_name']);





				//project logs
				$("#project_logs").empty();
				var logs_table = document.createElement("table");
				logs_table.setAttribute("id", "project_logs_table");
				var logs_thead = document.createElement("thead");

				var logs_heads = document.createElement('tr');
				var lheading_1 = document.createElement('th');
				lheading_1.innerHTML = "Log Text";
				var lheading_2 = document.createElement('th');
				lheading_2.innerHTML = "Log ID";
				var lheading_3 = document.createElement('th');
				lheading_3.innerHTML = "Log Date";
				var lheading_4 = document.createElement('th');
				lheading_4.innerHTML = "";

				logs_heads.append(lheading_1);
				logs_heads.append(lheading_2);
				logs_heads.append(lheading_3);
				logs_heads.append(lheading_4);

				logs_thead.append(logs_heads);
				logs_table.append(logs_thead);

				$("#project_logs").append(logs_table);

				$("#project_logs_table").DataTable({
					"processing": true, // for show progress bar
				"serverSide": true, // for process server side
				"filter": true, // this is for disable filter (search box)
				"orderMulti": false, // for disable multiple column at once
				"pageLength": 10,
				"language": {
					"emptyTable": "لا يوجد بيانات"
		},
				"ajax": {
					"url": "/Admin/project_logs?project_id=" + id,
				"type": "POST",
				dataSrc: function (data) {


					console.log("SERVER DATA : ", data)
				return data.data
			},
				"datatype": "json"
		},

				"columnDefs":
				[{
					"targets": [0],
				"searchable": true,
				"orderable": true
			}],


				"columns": [
				{"data": "log_text", "name": "log_text", "autoWidth": true },
				{"data": "log_id", "name": "log_id", "autoWidth": true },
				{"data": "log_date", "name": "log_date", "autoWidth": true },
				{
					data: null, render: function (data, type, row) {
					return '<button class="btn btn-danger delete" type="button">حذف</button>'
					//return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.stage_id + "'); >Delete</a>";
				}
			}
			]

	});


			//project_gross_marign

			$("#gross_marign").empty();
			var gross_table = document.createElement("table");
			gross_table.setAttribute("id", "project_gross_table");
			var gross_heads = document.createElement("tr");
			var gross_thead = document.createElement("thead");


			var gheading_1 = document.createElement('th');
			gheading_1.innerHTML = "Description";
			var gheading_2 = document.createElement('th');
			gheading_2.innerHTML = "Gross ID";
			var gheading_3 = document.createElement('th');
			gheading_3.innerHTML = "Date";
			var gheading_4 = document.createElement('th');
			gheading_4.innerHTML = "Quantity";
			var gheading_5 = document.createElement('th');
			gheading_5.innerHTML = "Amount";
			var gheading_6 = document.createElement('th');
			gheading_6.innerHTML = "Type";
			var gheading_7 = document.createElement('th');
			gheading_7.innerHTML = "Funder";
			var gheading_8 = document.createElement('th');
			gheading_8.innerHTML = "";

			gross_heads.append(gheading_1);
			gross_heads.append(gheading_2);
			gross_heads.append(gheading_3);
			gross_heads.append(gheading_4);
			gross_heads.append(gheading_5);
			gross_heads.append(gheading_6);
			gross_heads.append(gheading_7);
			gross_heads.append(gheading_8);


			gross_thead.append(gross_heads);
			gross_table.append(gross_thead);
			$("#gross_marign").append(gross_table);

			$("#project_gross_table").DataTable({
				"processing": true, // for show progress bar
			"serverSide": true, // for process server side
			"filter": true,// this is for disable filter (search box)
			"orderMulti": false, // for disable multiple column at once
			"pageLength": 10,
			"language": {
				"emptyTable": "لا يوجد بيانات"
		},
			"ajax": {
				"url": "/Admin/project_gross?project_id=" + id,
			"type": "POST",
			//dataSrc: function (data) {


				//	console.log("SERVER DATA : ", data)
				//	return data.data
				//},
				"datatype": "json"
		},

			"columnDefs":
			[{
				"targets": [0],
			"searchable": true,
			"orderable": true
			}],


			"columns": [
			{"data": "description", "name": "description", "autoWidth": true },
			{"data": "gross_margin_id", "name": "gross_margin_id", "autoWidth": true },
			{"data": "gross_date", "name": "gross_date", "autoWidth": true },
			{"data": "quantity", "name": "quantity", "autoWidth": true },
			{"data": "Amount", "name": "Amount", "autoWidth": true },
			{"data": "gross_margin_typename", "name": "gross_margin_typename", "autoWidth": true },
			{"data": "funder", "name": "user_assiocated", "autoWidth": true },
			{
				data: null, render: function (data, type, row) {
					//return '<button class="btn btn-danger delete" type="button">Delete</button>'
					return `<a class="btn btn-danger" onclick=delete_gross(${row.gross_margin_id}); >Delete</a>`;
				}
			}
		]

	});




		//project attachments
		$("#attachments").empty();
		var attachments_table = document.createElement("table");
		attachments_table.setAttribute("id", "project_attachments_table");
		var attachments_thead = document.createElement("thead");

		var attachments_heads = document.createElement('tr');
		var aheading_1 = document.createElement('th');
		aheading_1.innerHTML = "Attachment Name";
		var aheading_2 = document.createElement('th');
		aheading_2.innerHTML = "Attachment ID";
		var aheading_3 = document.createElement('th');
		aheading_3.innerHTML = "File";
		var aheading_4 = document.createElement('th');
		aheading_4.innerHTML = "";

		attachments_heads.append(aheading_1);
		attachments_heads.append(aheading_2);
		attachments_heads.append(aheading_3);
		attachments_heads.append(aheading_4);

		attachments_thead.append(attachments_heads);
		attachments_table.append(attachments_thead);
		$("#attachments").append(attachments_table);

		$("#project_attachments_table").DataTable({
			"processing": true, // for show progress bar
		"serverSide": true, // for process server side
		"filter": true,// this is for disable filter (search box)
		"orderMulti": false, // for disable multiple column at once
		"pageLength": 10,
		"language": {
			"emptyTable": "لا يوجد بيانات"
		},
		"ajax": {
			"url": "/Admin/project_attachment?project_id=" + id,
		"type": "POST",
			//dataSrc: function (data) {


			//	console.log("SERVER DATA : ", data)
			//	return data.data
			//},
			"datatype": "json"
		},

		"columnDefs":
		[{
			"targets": [0],
		"searchable": true,
		"orderable": true
			}],


		"columns": [
		{"data": "attachment_name", "name": "attachment_name", "autoWidth": true },
		{"data": "attachment_id", "name": "attachment_id", "autoWidth": true },

		{
			data: null,
		render: function (data, type, row) {
			console.log(row);
		return `<a class='text-decoration-none me-3' href='../project_attachments/${row.attachment_name}' target='_blank' data-gallery='attachment-bg'><div class='bg-attachment'><div class='bg-holder rounded' target='_blank' style='background-image: url(../project_attachments/${row.attachment_name});'></div></div></a>`;

				}
			},
		{
			data: null, render: function (data, type, row) {
					//return '<button class="btn btn-danger delete" type="button">Delete</button>'
					return `<a class="btn btn-danger" onclick=delete_attachment(${row.attachment_id}); >Delete</a>`;
				}
			}
	]

	});

}



	@*function add_attachments(kanban_item_full_id) {
	//getting the id number only
	var word_list = kanban_item_full_id.split("_");
	var id_number = word_list[word_list.length - 1];


	//creating the item from scratch
	var first_div = "<div class='d-flex align-items-center mb-3' id='attachment_item'></div>"
	var second_div = "<a class='text-decoration-none me-3' id='href_id' href='' target='_blank' data-gallery='attachment-bg'></div>";
	var third_div = "<div class='bg-attachment'></div>";
	var fourth_div = "<div class='bg-holder rounded' id='bg_style' style='background-image: url(../Content/assets/img/kanban/3.jpg);'></div>";
	var fifth_div = "<div class='flex-1 fs--2'></div>";
	var sixth_div = "<h6 class='mb-1'> <a class='text - decoration - none' id='attach_name' target='_blank' href='../Content/assets/img/kanban/3.jpg' data-gallery='attachment - title'>final-img.jpg</a></h6>";



	$("#attachment_column").append(first_div);
	var temp_item = $("#attachment_item");
	temp_item.append(second_div);
	temp_item.append(fifth_div);
	temp_item.find("#href_id").append(third_div);
	temp_item.find(".bg-attachment").append(fourth_div);
	temp_item.find(".flex-1").append(sixth_div);

	var main_item = temp_item.clone();
	$("#attachment_column").empty();
	//$("#attachment_header").empty();
	//$("#attachment_column").append(header); //adding the attachment header after deleting it from .empty() method.


	//$.ajax({
	//	type: 'GET',
	//	url: '../api/attachemnts/' + id_number,
	//	dataType: 'json',
	//	success: function (data) {
	//		if (data == null) { return false; }

	//		$.each(data, function (index, val) {
	//			var item = main_item.clone();
	//			item.attr("id", "attachment_item_" + id_number);
	//			item.find("#href_id").attr("href", "../project_attachments/" + val);
	//			item.find("#attach_name").attr("href", "../project_attachments/" + val);
	//			item.find("#attach_name").text(val);
	//			item.find("#bg_style").attr("style", "background-image: url(../project_attachments/" + val + ");");
	//			$("#attachment_column").append(item);

	//		});
	//		$("#attachment_column").append("<hr class='my-4'>");
	//	}

	//});

	var all_projects = @Html.Raw(Json.Encode(Model.projects));
		var project = all_projects[id_number];

		if (project['files'] == null)
		return false;

		$.each(project['files'], function (index, val) {
		var item = main_item.clone();
		item.attr("id", "attachment_item_" + id_number);
		item.find("#href_id").attr("href", "../project_attachments/" + val.attachment_name);
		item.find("#attach_name").attr("href", "../project_attachments/" + val.attachment_name);
		item.find("#attach_name").text(val.attachment_name);
		item.find("#bg_style").attr("style", "background-image: url(../project_attachments/" + val.attachment_name + ");");
		$("#attachment_column").append(item);
		//$("#attachment_column").append("<hr class='my-4'>");
	})

}*@

			function delete_gross(gross_id) {

	var project_id = $("#project_id").val();
			$.ajax({
				type: 'DELETE',
			url: `../Admin/delete_proj_gross?project_id=${project_id}&gross_id=${gross_id}`,
			dataType: 'json',
			success: function (data) {
				alert('gross marign has been deleted successfully!');
			var table = $("#project_gross_table").DataTable();
			table.draw();
		}

	});

}

			function delete_attachment(attachment_id) {

	var project_id = $("#project_id").val();
			$.ajax({
				type: 'DELETE',
			url: `../Admin/delete_proj_attachment?project_id=${project_id}&attachment_id=${attachment_id}`,
			dataType: 'json',
			success: function (data) {
				alert('Attachmnet has been deleted successfully!');
			var table = $("#project_attachments_table").DataTable();
			table.draw();
		}

	});

}

			function show_dropdown() {

				$("#institute_div").show();
			$("#institute_edit_button").hide();
			$("#institute_save_button").show();
			$("#institute_cancel_button").show();


}

			function cancel_edit() {
				$("#institute_div").hide();
			$("#institute_edit_button").show();
			$("#institute_save_button").hide();
			$("#institute_cancel_button").hide();
}

			function save_field(element_id) {
	var value = $(`#${element_id}`).val();
			var project_id = $("#project_id").val();
			console.log(value);
	//if (!alert("are you sure you want to update institute?")) {
				//	return false;
				//}

				$.ajax({
					type: 'POST',
					url: `../Admin/edit_project_fields?project_id=${project_id}&value=${value}&field_name=${element_id}`,
					dataType: 'json',
					success: function (data) {
						alert('project has been updated successfully!');

					}

				});

}

			function show_startdatepicker() {
				$("#plannedstartdate").show();
			$("#startdate_save_button").show();
			$("#startdate_cancel_button").show();
			$("#startdate_edit_button").hide();

}

			function show_enddatepicker() {
				$("#plannedenddate").show();
			$("#enddate_save_button").show();
			$("#enddate_cancel_button").show();
			$("#enddate_edit_button").hide();

}

			function cancel_startdate() {
				$("#plannedstartdate").hide();
			$("#startdate_save_button").hide();
			$("#startdate_cancel_button").hide();
			$("#startdate_edit_button").show();
}

			function cancel_enddate() {
				$("#plannedenddate").hide();
			$("#enddate_save_button").hide();
			$("#enddate_cancel_button").hide();
			$("#enddate_edit_button").show();
}

			function show_deadline() {
				$("#deadline_date").show();
			$("#deadline_save_button").show();
			$("#deadline_cancel_button").show();
			$("#deadline_edit_button").hide();

}

			function cancel_deadline() {
				$("#deadline_date").hide();
			$("#deadline_save_button").hide();
			$("#deadline_cancel_button").hide();
			$("#deadline_edit_button").show();
}

			function show_desc() {
				$("#desc_save_button").show();
			$("#desc_cancel_button").show();
			$("#edit_desc").show();
			$("#desc_edit_button").hide();

}

			function cancel_desc() {
				$("#desc_save_button").hide();
			$("#desc_cancel_button").hide();
			$("#edit_desc").hide();
			$("#desc_edit_button").show();
}

			function show_cost() {
				$("#cost_save_button").show();
			$("#cost_cancel_button").show();
			$("#cost_edit").show();
			$("#cost_edit_button").hide();


}

			function cancel_cost() {
				$("#cost_save_button").hide();
			$("#cost_cancel_button").hide();
			$("#cost_edit").hide();
			$("#cost_edit_button").show();

}

			function show_dropdown_stage() {
				$("#stage_save_button").show();
			$("#stage_cancel_button").show();
			$("#stage_edit").show();
			$("#stage_edit_button").hide();


}

			function cancel_stage() {
				$("#stage_save_button").hide();
			$("#stage_cancel_button").hide();
			$("#stage_edit").hide();
			$("#stage_edit_button").show();

}






