using Car_Rental.Data.Interfaces;

namespace Car_Rental.Data.Classes
{
    public static class EntityMatcher
    {
        public static void MatchAndInsert<TOuter, TInner>(
            IData dataService,
            Func<TOuter, string> outerKeySelector,
            Func<TInner, string> innerKeySelector,
            Action<TOuter, TInner> assignmentAction)
        {
            var outerEntities = dataService.GetDataObjectsOfType<TOuter>();
            var innerEntities = dataService.GetDataObjectsOfType<TInner>();

            var joinedEntities = from outerEntity in outerEntities
                                 join innerEntity in innerEntities
                                 on outerKeySelector(outerEntity) equals innerKeySelector(innerEntity)
                                 select new { OuterEntity = outerEntity, InnerEntity = innerEntity };

            foreach (var item in joinedEntities)
            {
                assignmentAction(item.OuterEntity, item.InnerEntity);
            }
        }
    }
}