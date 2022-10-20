using UnityEngine;

// <T> pode ser de qualquer tipo.
public class Singleton<T> : MonoBehaviour where T: Component
{
    // A instância só pode ser acessada pelo getter.
    private static T m_instance;
    public static bool m_IsQuitting;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                // Certificando - se de que não há outras instâncias do mesmo tipo na memória.
                m_instance = FindObjectOfType<T>();

                if(m_instance == null)
                {
                    // Certificando-se de que não há outras instâncias do mesmo tipo na memória.
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    m_instance = obj.AddComponent<T>();
                }
            }

            return m_instance;
        }
    }

    // // Virtual Awake () que pode ser substituído em uma classe derivada.
    public virtual void Awake()
    {
        if(m_instance == null)
        {
            // Se nulo, esta instância é agora a instância Singleton do tipo atribuído.
            m_instance = this as T;

            // Certificando-se de que minha instância Singleton persistirá na memória em todas as cenas.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Destrói a instância atual porque ela deve ser uma duplicata.
            Destroy(gameObject);
        }
    }
}