using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Enums;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DBR.Web.Pages.Cases;

public partial class CreateIncident
{
	[Parameter] public string? CaseId { get; set; }

	[Inject] IWebHostEnvironment Environment { get; set; } = default!;

	[Inject] IService<Incident, IncidentInputModel, IncidentDTO> IncidentService { get; set; } = default!;

	[Inject] IService<Video, VideoInputModel, VideoDTO> VideoService { get; set; } = default!;

	[Inject] IService<Image, ImageInputModel, ImageDTO> ImageService { get; set; } = default!;

	[Inject] IService<Invoice, InvoiceInputModel, InvoiceDTO> InvoiceService { get; set; } = default!;

	[Inject] IService<Attachment, AttachmentInputModel, AttachmentDTO> AttachmentService { get; set; } = default!;

	[Inject] IService<Case, CaseInputModel, CaseDTO> CaseService { get; set; } = default!;

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	readonly IncidentInputModel incidentInputModel = new();
	List<Tuple<Guid, IBrowserFile>>? browserFiles;
	string? errorMessage;
	bool isInvalidPath;
	bool isLoading;

	protected override async Task OnInitializedAsync()
	{
		if (string.IsNullOrWhiteSpace(CaseId))
		{
			errorMessage = "Hov, der er ingenting her.";
			isInvalidPath = true;

			return;
		}

		bool isCaseIdValid = Guid.TryParse(CaseId, out Guid caseId);

		if (!isCaseIdValid)
		{
			errorMessage = "Den valgte sag er ikke gyldig.";
			isInvalidPath = true;

			return;
		}

		ResponseDTO<CaseDTO> @case = await CaseService.GetByIdAsync(caseId, selector: x => new Case { Id = x.Id }, cancellationToken: CancellationToken);

		if (!@case.Success)
		{
			errorMessage = @case.ErrorMessage;
			isInvalidPath = true;
		}
		else
		{
			incidentInputModel.CaseId = @case.Content!.Id;
		}
	}

	void OnInputFileChange(InputFileChangeEventArgs e)
	{
		IReadOnlyList<IBrowserFile> files = e.GetMultipleFiles();

		if (files?.Any() ?? false)
		{
			browserFiles ??= new();

			foreach (IBrowserFile file in files)
			{
				browserFiles.Add(Tuple.Create(Guid.NewGuid(), file));
			}
		}
	}

	//void RemoveBrowserFileInState(Guid id)
	//{
	//	if (browserFiles is null)
	//	{
	//		errorMessage = "Kunne ikke slette fil, da der findes ingen filer.";

	//		return;
	//	}

	//	Tuple<Guid, IBrowserFile>? browserFileToDelete = browserFiles.Find(x => x.Item1 == id);

	//	if (browserFileToDelete is null)
	//	{
	//		errorMessage = "Kunne ikke slette fil, da filen ikke fandtes.";

	//		return;
	//	}

	//	browserFiles.Remove(browserFileToDelete);
	//}

	async Task CreateIncidentAsync()
	{
		isLoading = true;

		ResponseDTO<IncidentDTO> createdIncident = await IncidentService.CreateAsync(incidentInputModel, CancellationToken);

		if (!createdIncident.Success)
		{
			errorMessage = createdIncident.ErrorMessage;

			return;
		}

		if (browserFiles is not null)
		{
			foreach (Tuple<Guid, IBrowserFile> attachment in browserFiles)
			{
				if (attachment.Item2.Size <= 0)
				{
					errorMessage = "Fejl ved oprettelse af hændelse - filupload fejlede.";

					return;
				}

				string uploadPath = Path.Combine(Environment.WebRootPath, "uploads");

				if (!Directory.Exists(uploadPath))
				{
					Directory.CreateDirectory(uploadPath);
				}

				await using FileStream fileStream = new(Path.Combine(uploadPath, attachment.Item2.Name), FileMode.Create);
				await attachment.Item2.OpenReadStream(2147483648, CancellationToken).CopyToAsync(fileStream, CancellationToken);

				AttachmentInputModel attachmentInputModel = new()
				{
					IncidentId = createdIncident.Content!.Id,
				};

				if (attachment.Item2.ContentType.Contains("image"))
				{
					attachmentInputModel.Type = AttachmentType.Image;

					ImageInputModel imageInputModel = new()
					{
						ImageURL = Path.Combine("uploads", attachment.Item2.Name)
					};

					ResponseDTO<ImageDTO> createdImage = await ImageService.CreateAsync(imageInputModel, CancellationToken);

					if (!createdImage.Success)
					{
						errorMessage = createdImage.ErrorMessage;

						return;
					}

					attachmentInputModel.ImageId = createdImage.Content!.Id;
				}
				else if (attachment.Item2.ContentType.Contains("video"))
				{
					attachmentInputModel.Type = AttachmentType.Video;

					VideoInputModel videoInputModel = new()
					{
						VideoURL = Path.Combine("uploads", attachment.Item2.Name)
					};

					ResponseDTO<VideoDTO> createdVideo = await VideoService.CreateAsync(videoInputModel, CancellationToken);

					if (!createdVideo.Success)
					{
						errorMessage = createdVideo.ErrorMessage;

						return;
					}

					attachmentInputModel.VideoId = createdVideo.Content!.Id;
				}
				else
				{
					attachmentInputModel.Type = AttachmentType.Invoice;

					InvoiceInputModel invoiceInputModel = new()
					{
						InvoiceURL = Path.Combine("uploads", attachment.Item2.Name)
					};

					ResponseDTO<InvoiceDTO> createdInvoice = await InvoiceService.CreateAsync(invoiceInputModel, CancellationToken);

					if (!createdInvoice.Success)
					{
						errorMessage = createdInvoice.ErrorMessage;

						return;
					}

					attachmentInputModel.InvoiceId = createdInvoice.Content!.Id;
				}

				ResponseDTO<AttachmentDTO> createdAttachment = await AttachmentService.CreateAsync(attachmentInputModel, CancellationToken);
			}
		}

		NavigationManager.NavigateTo($"sager/detaljer/{CaseId}");

		isLoading = false;
	}

	void NavigateBack()
	{
		NavigationManager.NavigateTo((string.IsNullOrWhiteSpace(CaseId) || isInvalidPath) ? "sager" : $"sager/detaljer/{CaseId}");
	}
}