using System;


namespace OOP_Paint {
    public readonly struct ConstructorOperationStatus {
        public enum OperationStatus {
            None,
            Canselled,
            Continious,
            Finished,

        }

        public readonly OperationStatus Result;
        public readonly String OperationMessage;



        //???Слишком сложна!
        public static bool operator ==(ConstructorOperationStatus c1, ConstructorOperationStatus c2) {
            if (c1.Result == c2.Result && c1.OperationMessage == c2.OperationMessage) {
                return true;
            }
            else return false;
        }
        public static bool operator !=(ConstructorOperationStatus c1, ConstructorOperationStatus c2) {
            if (c1.Result == c2.Result && c1.OperationMessage == c2.OperationMessage) {
                return false;
            }
            else return true;
        }



        public ConstructorOperationStatus(OperationStatus _result, String _message) {
            Result = _result;
            OperationMessage = _message;
        }

    }
}
