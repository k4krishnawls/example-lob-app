

type BadRequestResponseModel = {
  errorType: BadRequestType;
  fieldErrors?: { field: string, errors: string[] }[],
  generalError?: string;
};

export enum BadRequestType {
  GeneralError = 1,
  FieldErrors = 2
}

export const parseBadRequestResponse = async (response: Response): Promise<BadRequestResponseModel> => {
  const json = await response.json();
  if (json.errorType! == BadRequestType.GeneralError) {
    return {
      errorType: BadRequestType.GeneralError,
      generalError: json.generalError
    };
  }
  else {
    const sourceErrors = (json.fieldErrors ?? {});
    return {
      errorType: BadRequestType.FieldErrors,
      generalError: "One or more fields had an error",
      fieldErrors: Object.keys(sourceErrors).map(k => ({
        field: k,
        errors: sourceErrors[k]
      }))
    };
  }
};
