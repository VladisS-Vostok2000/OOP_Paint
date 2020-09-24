using System;


namespace OOP_Paint {
    public readonly struct ConstructorOperationResult {
        public enum OperationStatus {
            None,
            Canselled,
            Continious,
            Finished,
        }
        public readonly OperationStatus Result;
        public readonly String OperationMessage;
        public ConstructorOperationResult(OperationStatus _result, String _message) {
            Result = _result;
            OperationMessage = _message;
        }


    }
}
