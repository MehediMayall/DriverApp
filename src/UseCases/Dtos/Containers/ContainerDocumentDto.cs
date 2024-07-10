namespace UseCases.Dtos;

public record ContainerDocumentDto(DocumentID documentID, string DocumentName, string DocumentPath);

// Request - Primitive
public record DocumentResponseDto(int DocumentID, string DocumentName, string DocumentPath);

