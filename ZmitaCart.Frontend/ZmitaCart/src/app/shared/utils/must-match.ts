import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const mustMatch = (controlName: string, matchingControlName: string): ValidatorFn => {
	return (control: AbstractControl): ValidationErrors | null => {
		const password = control.get(controlName);
		const confirm_password = control.get(matchingControlName);

		if (password.value === '') {
			return;
		}

		if (password?.value !== confirm_password?.value) {
			confirm_password.setErrors({ mustMatch: true });
		} else {
			confirm_password.setErrors(null);
		}

		return null;
	};
};
