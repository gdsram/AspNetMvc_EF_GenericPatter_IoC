using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IManyToManyRepository<T1, T2> where T1 : BaseEntity
                                                   where T2 : BaseEntity
    {
        void InsertWithoutData(T1 t1, T2 t2, string navigationProperty);
        void InsertWithData(int entity1ID, int entity2ID, string navigationProperty);
        void InsertWithData(int entity1ID, List<int> entities2ID, string navigationProperty);
        void UpdateRelationship(int oldIDT1, int oldIDT2, int newIDT2, string navigationProperty);
        void UpdateRelationship(int oldIDT1, List<int> oldIDT2List, List<int> newIDT2List, string navigationProperty);
        void UpdateRelationship(int idT1, List<int> newIDT2List, string navigationProperty);
        void DeleteRelationship(int entity1ID, int entity2ID, string navigationProperty);
        void DeleteRelationship(int entity1ID, List<int> listID, string navigationProperty);
        void DeleteAllRelationship(int entity1ID, string navigationProperty);
        List<DTOGenericObject> GetT1ByT2(int entity2ID, string navigationProperty);
    }
}
