using System;


namespace OOP_Paint {
    public readonly struct ConstructorOperationStatus {
        public enum OperationStatus {
            None,
            Canselled,
            Continious,
            Exeption,
            Finished,

        }

        public readonly OperationStatus Result;
        public readonly string OperationMessage;



        public static bool operator ==(ConstructorOperationStatus c1, ConstructorOperationStatus c2) {
            return c1.Result == c2.Result && c1.OperationMessage == c2.OperationMessage;
        }
        public static bool operator !=(ConstructorOperationStatus c1, ConstructorOperationStatus c2) {
            return c1.Result != c2.Result || c1.OperationMessage != c2.OperationMessage;
        }
        public override bool Equals(object obj) {
            return obj != null && GetType().Equals(obj.GetType()) && this == (ConstructorOperationStatus)obj;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }



        public ConstructorOperationStatus(OperationStatus _result, string _message) {
            Result = _result;
            OperationMessage = _message;
        }

    }
}
