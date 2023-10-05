﻿using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;
using Car_Rental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Business.Classes;
public abstract class BaseService
{
    protected readonly DataManagement _dataManagement;

    protected BaseService(DataManagement dataManagement)
    {
        _dataManagement = dataManagement;
    }
    public DataManagement DataManagement { get { return _dataManagement; } }
    public void AddDataObject(IDataObject dataObject) => _dataManagement.AddDataObject(dataObject);

}
