using Car_Rental.Data.Interfaces;

namespace Car_Rental.Data.Classes
{
    public static class DataMatching
    {
        public static void MatchAndInsert<TOuter, TInner>(
            IEnumerable<TOuter> outerEntities,
            IEnumerable<TInner> innerEntities,
            Func<TOuter, string> outerKeySelector,
            Func<TInner, string> innerKeySelector,
            Action<TOuter, TInner> assignmentAction)
        {
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