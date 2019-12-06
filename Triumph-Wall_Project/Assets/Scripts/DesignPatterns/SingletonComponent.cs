//Código extraido de: http://www.unitygeek.com/unity_c_singleton/
//Blog explicativo de la implementación del patrón Singleton en C# Unity
using UnityEngine;

//Clase Template
public class SingletonComponent<T> : MonoBehaviour where T : Component
{
    //Instancia
    private static T instance;

    //Acceso a la instancia mediante un getter que asegura su presencia en escena
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
                }
            }
            return instance;
        }
    }

    //Función a heredar para evitar su destrucción o la duplicidad del singletone.
    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

/*Ejemplo de implementación:

Hará falta implementar el siguiente código en la clase que herede

public class GameController : SingletonComponent<GameController>
{

    public override void Awake()
    {
        base.Awake();
    }

}
*/
