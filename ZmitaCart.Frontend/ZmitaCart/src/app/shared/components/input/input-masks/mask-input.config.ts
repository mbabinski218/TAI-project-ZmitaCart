import { createMask } from '@ngneat/input-mask';

export type InputMaskType = 'post-code' | 'telephone';

export const POSTCODE_MASK = createMask({
	mask: '99-999',
	autoUnmask: true,
	jitMasking: true
});

export const TELEPHONE_MASK = createMask({
	mask: '999-999-999',
	autoUnmask: true,
	jitMasking: true
});