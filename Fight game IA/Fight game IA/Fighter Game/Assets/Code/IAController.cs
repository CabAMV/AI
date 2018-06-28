using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IA controller
public class IAController : MonoBehaviour
{
    [SerializeField]
    int windowSize = 3; // Number of char to use as prediction

    Predictor predictor;

    string totalElecciones;
    string eleccionesPredecir;
    string eleccionesRegistrar;

    // Use this for initialization
    void Start ()
    {
        predictor = new Predictor("1234");
        totalElecciones = "";
        eleccionesPredecir = "";
        eleccionesRegistrar = "";
    }

    // Print the table's data
    public string PrintTablesData()
    {
        return predictor.PrintData();
    }

    // Takes the move that wpuld counter the player's movement if its prediction is true
    public string nextMove ()
    {
        return predictor.counterMovement(eleccionesPredecir);
    }

	// Save the player's char for data collecting
    public void savechar (string opcion)
    {
        totalElecciones += opcion;

        if (totalElecciones.Length - windowSize < 0)
        {
            eleccionesPredecir += opcion;
        }
        else
        {
            eleccionesPredecir = totalElecciones.Substring(totalElecciones.Length - windowSize);
        }

        if (totalElecciones.Length - windowSize - 1 < 0)
        {
            eleccionesRegistrar += opcion;
        }
        else
        {
            eleccionesRegistrar = totalElecciones.Substring(totalElecciones.Length - (windowSize + 1));
            predictor.RegisterSequence(eleccionesRegistrar);
        }
    }
}
