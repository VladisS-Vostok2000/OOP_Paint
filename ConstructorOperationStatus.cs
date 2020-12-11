using System;


namespace OOP_Paint {
    public readonly struct ConstructorOperationStatus {
        public enum OperationStatus {
            None,
            Canselled,
            Continious,
            Exception,
            Finished,
        }

        public readonly OperationStatus Result;
        public readonly String OperationMessage;



        public static Boolean operator ==(ConstructorOperationStatus c1, ConstructorOperationStatus c2) {
            return c1.Result == c2.Result && c1.OperationMessage == c2.OperationMessage;
        }
        public static Boolean operator !=(ConstructorOperationStatus c1, ConstructorOperationStatus c2) {
            return c1.Result != c2.Result || c1.OperationMessage != c2.OperationMessage;
        }
        public override Boolean Equals(Object obj) {
            return obj != null && GetType().Equals(obj.GetType()) && this == (ConstructorOperationStatus)obj;
        }
        public override Int32 GetHashCode() {
            return base.GetHashCode();
        }



        public ConstructorOperationStatus(OperationStatus _result, String _message) {
            Result = _result;
            OperationMessage = _message;
        }

    }
}
