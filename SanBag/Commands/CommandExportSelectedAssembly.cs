using LibSanBag;
using SanBag.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SanBag.Commands
{
    public class CommandExportSelectedAssembly : ICommand
    {
        private ScriptCompiledBytecodeResourceViewModel viewModel;

        event EventHandler ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public CommandExportSelectedAssembly(ScriptCompiledBytecodeResourceViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            var items = parameter as System.Collections.IList;
            var fileRecords = items.Cast<FileRecord>().ToList();

            viewModel.ExportRecordsAsAssemblies(fileRecords);
        }
    }
}
