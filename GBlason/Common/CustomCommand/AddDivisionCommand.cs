﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GBL.Repository.Resources;
using GBlason.ViewModel;
using GBlason.ViewModel.Contract;
using GBlasonLogic.Rules;

namespace GBlason.Common.CustomCommand
{
    public class AddDivisionCommand : CommandGeneric
    {
        public AddDivisionCommand(Type ownertype)
            : base("Add Division", ownertype)
        {
        }

        public override bool CanExecute(object parameter, IInputElement target)
        {
            if(GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile!=null)
                if (GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile.CurrentlySelectedComponent!=null)
                    return DivisionRules.IsAllowed((DivisionType)parameter,
                                           GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile.
                                               CurrentlySelectedComponent.OriginObject);
            return false;
        }

        public override void Execute(CommandParameter parameter, IInputElement target)
        {
            //var divisibleArea = GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile.
            //                        CurrentlySelectedComponent as IDivisible;
            //if (divisibleArea == null) return;
            //divisibleArea.AddDivision((DivisionType)parameter);
            //base.Execute(parameter, target);
        }

        public override void Undo()
        {

        }
    }
}
