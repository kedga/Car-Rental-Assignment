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
            Console.WriteLine($"Number of outer entities: {outerEntities.ToList().Count}");
            Console.WriteLine($"Number of inner entities: {innerEntities.ToList().Count}");

            //var outerEntities = await dataService.GetDataObjectsOfType<TOuter>();
            //var innerEntities = await dataService.GetDataObjectsOfType<TInner>();

            var joinedEntities = from outerEntity in outerEntities
                                 join innerEntity in innerEntities
                                 on outerKeySelector(outerEntity) equals innerKeySelector(innerEntity)
                                 select new { OuterEntity = outerEntity, InnerEntity = innerEntity };
            Console.WriteLine("Join Debugging:");
            Console.WriteLine($"Number of outer entities: {outerEntities.ToList().Count}");
            Console.WriteLine($"Number of inner entities: {innerEntities.ToList().Count}");
            Console.WriteLine($"Number of joined entities: {joinedEntities.Count()}");

            foreach (var item in joinedEntities)
            {
                Console.WriteLine($"Outer Key: {outerKeySelector(item.OuterEntity)}, Inner Key: {innerKeySelector(item.InnerEntity)}");
                Console.WriteLine($"Outer Entity: {item.OuterEntity}");
                Console.WriteLine($"Inner Entity: {item.InnerEntity}");

                assignmentAction(item.OuterEntity, item.InnerEntity);
            }
        }
    }
}