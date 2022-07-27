using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceble
{
    Vector2 position {get; set;} // позиция вышки
    int attack {get; set;} // размер атаки
    int poison {get; set;} // количество урона от яда, если есть
    bool magic {get; set;} // вышка магического типа?
    bool isDeleted {get; set;} // нужно ли удалить объект?
}
