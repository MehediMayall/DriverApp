namespace UseCases.Dtos;

public record DocumentImageDto
(
    int ProNumber, 
    string LegType,
    string ImagesInBase64,
    int CurrentImageIndex,
    int NumberOfImages
);