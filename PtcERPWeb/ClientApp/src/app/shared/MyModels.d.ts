interface IIdCount {
  FormateId: number;
  VandorName: string;
  DecviceName: string;
  FormateName: string;
  BarcodeId: string;
  BarcodeValue: string;
  Selected?: boolean;
  WithBarcodes: boolean;
  SetCount: number;
  Count: number;
  StockOrderId?:string
  StockUnitName?: string
  StockUnitId?: number
}
interface IIdName {
  Name: string;
  Id: string;
  Items?: IIdName[]
  Barcodes
}
interface IPick {
  PickOrderId: string;
  Selected?: boolean;


}

interface IsResult {
  Success: boolean;
  Message: string;
}
interface IStockOrderSub {
  BarcodeId: string;
  StockOrderSubId: string,
  StockOrderId: string,
}
interface IDictionary<T> {
  [Key: string]: T;
}
interface IRReturnOrderAdd {
    chreturned: () => void;
		PickOrderSubId: any;
		BarcodeId?: any;
		BarcodeValue: string;
		FormateName: string;
		VandorName: string;
		DeviceName: string;
		FormateId: number;
  WithBarcodes: boolean;
  StockUnitName: string;

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
	}
interface IdelCheck{
  TrackId?: string;
  ApplyNumber?: string;
}
interface RReturnOrder {
  ReturnOrderId: any;
  ReturnDate?: Date;
  CustomerName: string;
  ReturnPrice?: number;
  ItemCount: number;
  SalePrice?: number;
  CustomerConnecter: string;
  OrgPrice?: number;
  ApplyNumber: string;
}
interface IC{
  CreateId: string;
  CreateDate: Date;
}
interface IU {
  UpdateId: string;
  UpdateDate: Date;
}
interface ICU extends IC, IU { }

