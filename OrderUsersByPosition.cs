
/*
Пример сортировки пользователей по "возрастанию" должности.
Считает исходя из кол-ва вышестоящих руководителей
*/

public virtual void OrderUsersExample (Context context){
    //Создаем Temp лист. Нужен в случае, если требуется более сложная логика, => например собрать коллекцию из нескольких списков
    var allUsers = new HashSet<EleWise.ELMA.Security.Models.User> ();
    allUsers.AddAll (context.UsersList);
    //IDictionary с пользователем и глубиной (Depth) по кол-ву начальников выше
    IDictionary<EleWise.ELMA.Security.Models.User, int> sortedUsers = new Dictionary<EleWise.ELMA.Security.Models.User, int> ();
    foreach (var tmpSigner in allUsers) {
        var position = PublicAPI.Portal.Security.User.GetUserPositions (tmpSigner).FirstOrDefault ();
        if (position != null) {
            int chiefsCount = PublicAPI.Portal.Security.OrganizationItem.GetUserChiefsHierarchy (tmpSigner).GetUsersHierarchy (position).Cast<Security.Models.User> ().Count ();
            sortedUsers.Add (tmpSigner, chiefsCount);
        }
    }
    context.AllApproversOrdered.AddAll (context.PreApprovers);
    foreach (var user in sortedUsers.OrderByDescending (su => su.Value)) {
        context.AllUsersOrdered.Add (user.Key);
    }
}
