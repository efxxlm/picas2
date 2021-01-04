import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-controversia-contractua',
  templateUrl: './form-registrar-controversia-contractua.component.html',
  styleUrls: ['./form-registrar-controversia-contractua.component.scss']
})
export class FormRegistrarControversiaContractuaComponent implements OnInit {
  addressForm = this.fb.group({
    contrato: null,
  });
  contratosArray:any = [];
  nombreContratista: any;
  tipoIdentificacion: any;
  numIdentificacion: any;
  tipoIntervencion: any;
  valorContrato: any;
  plazoContrato: any;
  fechaInicioContrato: any;
  fechaFinalizacionContrato: any;
  contratoId: any;

  constructor(  private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService, private polizaService: PolizaGarantiaService) { }

  ngOnInit(): void {
    this.loadContractList();
  }

  loadContractList(){
    this.services.GetListContratos().subscribe(data=>{
      this.contratosArray = data;
    });
  }
  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  moduleChange(){
  }
  seleccionAutocomplete(id:any){
    this.addressForm.value.contrato = id;
    this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(resp_0=>{
      this.nombreContratista = resp_0[0].nombreContratista;
      this.tipoIdentificacion = resp_0[0].tipoDocumento;
      this.numIdentificacion = resp_0[0].numeroIdentificacion;
      this.valorContrato = resp_0[0].valorContrato;
    });
    this.services.GetVistaContratoContratista(id).subscribe((resp_1:any)=>{
      this.tipoIntervencion = resp_1.tipoIntervencion;
      this.plazoContrato = resp_1.plazoFormat;
      this.fechaInicioContrato = resp_1.fechaInicioContrato;
      this.fechaFinalizacionContrato = resp_1.fechaFinContrato;
    });
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  contratoSearch(contrato){

  }
}
