function initializeDataTable(tableId) {
	$(document).ready(function () {
		var table = $(tableId).DataTable({
			'lengthChange': false,
			'buttons': {
				'dom': {
					'button': {
						'className': 'btn btn-danger'
					}
				},
				'buttons': [
					{
						'extend': 'csv',
						'className': 'btn dbr-primary-button',
						'text': 'Download CSV'
					},
					{
						'extend': 'excel',
						'className': 'btn dbr-primary-button',
						'text': 'Download Excel'
					},
					{
						'extend': 'pdf',
						'className': 'btn dbr-primary-button',
						'text': 'Download PDF'
					},
					{
						'extend': 'print',
						'className': 'btn dbr-primary-button',
						'text': 'Udskriv'
					},
					{
						'extend': 'copy',
						'className': 'btn dbr-primary-button',
						'text': 'Kopier til udklip'
					}
				]
			},
			'responsive': true,
			'language': {
				'buttons': {
					'copyTitle': 'Kopier til udklip',
					'copySuccess': {
						'_': '%d elementer kopieret til udklipsholderen',
						'1': '1 element kopieret til udklipsholderen'
					}
				},
				'emptyTable': 'Ingen data fundet',
				'info': 'Viser række _START_-_END_ (side _PAGE_ af _PAGES_)',
				'infoEmpty': '',
				'infoFiltered': '(filtreret)',
				'zeroRecords': 'Ingen data fundet',
				'thousands': '.',
				'search': 'Søg:',
				'lengthMenu': 'Vis _MENU_ rækker',
				'paginate': {
					'first': 'Første side',
					'last': 'Sidste side',
					'next': 'Næste',
					'previous': 'Forrige'
				}
			}
		});

		table.buttons().container().appendTo('#datatable_id_wrapper .col-md-6:eq(0)');
		$('.dt-buttons').removeClass('btn-group');
		$('#datatable_id_filter').children('label').css({ 'margin-top': '5px' });
		$('#datatable_id_info').css({ 'margin-top': '-10px' });
		$('.btn').css({ 'margin-bottom': '3px' });
	});
}

function destroyDataTable(tableId) {
	$(document).ready(function () {
		$(tableId).DataTable().destroy();
	});
}

function generateThumbnail(videoId) {
	const video = document.getElementById(videoId);
	video.pause();
	video.currentTime = 5;
}