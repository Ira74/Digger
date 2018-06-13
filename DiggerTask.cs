using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Digger

{
    // Задания
    //    Terrain
    //Сделайте класс Terrain, реализовав ICreature.Сделайте так, чтобы он ничего не делал.
    //Player
    //Сделайте класс Player, реализовав ICreature.
    //Сделайте так, чтобы диггер шагал в разные стороны в зависимости от нажатой клавиши (Game.KeyPressed).
    //Убедитесь, что диггер не покидает пределы игрового поля.
    //Сделайте так, чтобы земля исчезала в тех местах, где прошел диггер.
    // Sack
    //    Сделайте класс Sack, реализовав ICreature.Это будет мешок с золотом.

    //   Мешок может лежать на любой другой сущности(диггер, земля, мешок, золото, край карты).
    //Если под мешком находится пустое место, он начинает падать.
    //Если мешок падает на диггера, диггер умирает, а мешок продолжает падать, пока не приземлится на землю, другой мешок, золото или край карты.
    //Диггер не может подобрать мешок, толкнуть его или пройти по нему.
    //Если мешок падает, а диггер находится непосредственно под ним и идет вверх, они могут "разминуться", и диггер окажется над мешком.Это поведение непросто исправить в существующей упрощенной архитектуре, поэтому считайте его нормальным.

    //Gold
    //Сделайте класс Gold, реализовав ICreature.

    //Мешок превращается в золото, если он падал дольше одной клетки игрового поля и приземлился на землю, на другой мешок или на золото.
    //Мешок не превращается в золото, а остаётся мешком, если он падал ровно одну клетку.
    //Золото никогда не падает.
    //Когда диггер собирает золото, ему начисляется 10 очков (через Game.Scores).

    //Решение (выполнено самостоятельно):

    class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    class Player : ICreature
    {
        public static int distanceX = 0;
        public static int distanceY = 0;

        public CreatureCommand Act(int x, int y)
        {
            switch (Game.KeyPressed)
            {
                case Keys.Up:
                    distanceX = 0;
                    distanceY = -1;
                    break;
                case Keys.Down:
                    distanceX = 0;
                    distanceY = 1;
                    break;
                case Keys.Left:
                    distanceX = -1;
                    distanceY = 0;
                    break;
                case Keys.Right:
                    distanceX = 1;
                    distanceY = 0;
                    break;
                default:
                    distanceX = 0;
                    distanceY = 0;
                    break;

            }

            if (!(x + distanceX >= 0 && x + distanceX < Game.MapWidth &&
                y + distanceY >= 0 && y + distanceY < Game.MapHeight) || Game.Map[x + distanceX, y + distanceY] is Sack)

            {
                distanceX = 0;
                distanceY = 0;

            }

            return new CreatureCommand() { DeltaX = distanceX, DeltaY = distanceY };
        }


        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }

    }
    //мешок с золотом

    class Sack : ICreature
    {
        public int count = 0;

        public CreatureCommand Act(int x, int y)
        {
            //1.Мешок может лежат на любой другой сущности
            //2.Если под мешком пустое место, он начинает падать
            //3.Если мешок пдает на диггера, диггер умирает
            //4.Диггер не может подобрат мешок, толкнуть его или пройти по нему

            if (y + 1 < Game.MapHeight)
            {
                if (((count > 0) && (Game.Map[x, y + 1] is Player)) || Game.Map[x, y + 1] == null)
                {
                    count++;
                    return new CreatureCommand { DeltaX = 0, DeltaY = 1 };
                }

            }
            if (count > 1 || y == Game.MapHeight)        //если мешок падал больше одной клетки
            {
                return new CreatureCommand { TransformTo = new Gold() };
            }
            else
                count = 0;

            return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
        }
        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }
    class Gold : ICreature                    //класс золото
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;              //добавляем 10 баллов
            }
            return true;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

}
