declare module server {
	interface RReturnOrderAdd {
		PickOrderSubId: any;
		BarcodeId?: any;
		BarcodeValue: string;
		FormateName: string;
		VandorName: string;
		DeviceName: string;
		FormateId: number;
		WithBarcodes: boolean;
		OrgPrice?: number;
		SalePrice?: number;
		ReturnCount: number;
		Returned: boolean;
		FReturned: boolean;
		ReturnPrice?: number;
		ReturnReason: string;
		PurchaseDate: Date;
		SaleDate?: Date;
		SApplyNumber: string;
		PApplyNumber: string;
		StockUnitName: string;
	}
}
