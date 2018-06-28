using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNeuronalNaves : MonoBehaviour {

    public static RedNeuronalNaves instance;

    private RedNeuronal red;
    public int numInput = 4, numHidden = 3, numOutput = 3;


    //Valores Iniciales de la red neuronal: se ponen casos, a la izquierda los inputs a la derecha los output deseables para dichos inputs.

    // INPUT  ->          //%frente   % derecha,  % izquierda,

    //OUTPUT: verticalMovement, horizontalRight, horizontalLeft
    private static float[,] entrenamiento ={
        { 1.0f, 0.0f, 0.0f,             1.0f, 0.0f, 0.0f},


        { 0.5f, 0.1f, 0.0f,             0.4f, 0.0f, 0.7f},
        { 0.5f, 0.0f, 0.1f,             0.4f, 0.7f, 0.0f},
        
        { 1.0f, 0.4f, 0.0f,             0.2f, 0.0f, 1.0f},//
        { 1.0f, 0.0f, 0.4f,             0.2f, 1.0f, 0.0f},//
        
        { 1.0f, 0.4f, 0.0f,             0.4f, 0.0f, 1.0f},
        { 1.0f, 0.0f, 0.4f,             0.4f, 1.0f, 0.0f},

    };





    void Start() 
    {
        instance = this;

        red = new RedNeuronal(numInput, numHidden, numOutput);
        EntrenarRed();
    }
    
    void EntrenarRed() 
    {
        float error = 1;
        int epoch = 0; // cantidad de veces que haces el entrenamiento

        while((error > 0.05f) && (epoch < 50000))
        {
            error = 0;
            epoch++;

            for (int i = 0; i < entrenamiento.GetLength(0); i++)
            {
                for (int j = 0; j < numInput; j++) 
                {
                    red.SetInput(j, entrenamiento[i, j]);
                }

                for (int j = numInput; j < numInput + numOutput; j++)
                {
                    red.SetOutputDeseado(j - numInput, entrenamiento[i,j]);
                }

                red.FeedForward();
                error += red.CalcularError();
                red.BackPropagation();
            }

            // VER COMO EVOLUCIONA EL ERROR A MEDIDA QUE AVANZAN LOS EPOCHS
            error /= entrenamiento.GetLength(0);
        }
    }

    public void ReentrenarRed(float [] inputs, float[] output)
    {
        float error = 1;
        int epoch = 0;

        while ((error > 0.1f) && (epoch < 5000))
        {
            epoch++;

            for (int j = 0; j < numInput; j++) 
            {
                red.SetInput(j, inputs[j]);
            }

            for (int j = numInput; j < numInput + numOutput; j++)
            {
                red.SetOutputDeseado(j - numInput, output[j]);
            }

            red.FeedForward();
            error = red.CalcularError();
            red.BackPropagation();
            // VER COMO EVOLUCIONA EL ERROR A MEDIDA QUE AVANZAN LOS EPOCHS
        }
    }

    public byte ConsultarAccion(float[] inputs)
    {

        for (int i = 0; i < inputs.Length; i++)
        {
            red.SetInput(i,inputs[i]);

        }
        red.FeedForward();
        return (byte)red.GetMaxOutputId();


    }

    public float[] CheckAction(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            red.SetInput(i, inputs[i]);
        }

        red.FeedForward();
        float[] outputs = new float[3];
        outputs[0] = GetOutput(0);
        outputs[1] = GetOutput(2);
        outputs[2] = GetOutput(1);

        return outputs;

    }


    public float GetOutput(int i)
    {
        return red.GetOutput(i);
    }

    public float GetMaxOutputId()
    {
        return red.GetMaxOutputId();
    }
}
