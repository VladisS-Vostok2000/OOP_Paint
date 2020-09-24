using System;


namespace OOP_Paint {
    public readonly struct ConstructorResult {
        public enum OperationStatus {
            None,
            Canselled,
            Continious,
            Finished,
        }
        public readonly OperationStatus Result;
        public readonly String OperationMessage;
        public ConstructorResult(OperationStatus _result, String _message) {
            Result = _result;
            OperationMessage = _message;
        }


    }
}
