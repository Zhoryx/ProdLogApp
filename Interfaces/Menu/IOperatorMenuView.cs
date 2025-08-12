using ProdLogApp.Models;

public interface IOperatorMenuView
{
    // Mensajes al usuario (error, info, confirmación)
    void ShowMessage(string message);

    // Reemplaza la lista completa de producciones mostradas
    void UpdateProductionList(List<Production> productions);

    // Añade una producción a la lista mostrada (sin recargar todo)
    void AddProductionToList(Production production);

    // Devuelve una nueva producción con los datos cargados en la vista
    Production GetProductionInput();
}
