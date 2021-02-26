import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { forkJoin, Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
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
    numeroContrato: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(10) ] ) ],
  });
  myFilter = new FormControl();
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
  estaEditando = false;
  selectedContract: any = '';
  filteredContrato: Observable<string[]>;
  constructor(  private fb: FormBuilder, 
    public dialog: MatDialog, 
    private services: ContractualControversyService, 
    private polizaService: PolizaGarantiaService,
    private budgetAvailabilityService: BudgetAvailabilityService) {
    this.services.GetListContratos().subscribe(
      result=>{ this.contratosArray=result;}
    );
    this.filteredContrato = this.myFilter.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
   }

  ngOnInit(): void {
    //this.loadContractList();
  }

  loadContractList(){
    this.services.GetListContratos().subscribe(data=>{
      this.contratosArray = data;
    });
  }
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();    
    if(value!="")
    {      
      let filtroportipo:string[]=[];
      this.contratosArray.forEach(element => {        
        if(!filtroportipo.includes(element.numeroContrato))
        {
          filtroportipo.push(element.numeroContrato);
        }
      });
      let ret= filtroportipo.filter(x=> x.toLowerCase().indexOf(filterValue) === 0);      
      return ret;
    }
    else
    {
      return [];
    }
    
  }
  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  seleccionAutocomplete(nombre: string){
    this.selectedContract = nombre;
    let lista: any[] = [];
    this.contratosArray.forEach(element => {
      if (element.numeroContrato) {
        lista.push(element);
      }
    });
    let ret = lista.filter(x => x.numeroContrato.toLowerCase() === nombre.toLowerCase());
    console.log(ret);
    this.contratoId = ret[0].contratoId;
    this.polizaService.GetListVistaContratoGarantiaPoliza(ret[0].contratoId).subscribe(resp_0=>{
      this.nombreContratista = resp_0[0].nombreContratista;
      this.tipoIdentificacion = resp_0[0].tipoDocumento;
      this.numIdentificacion = resp_0[0].numeroIdentificacion;
      this.valorContrato = resp_0[0].valorContrato;
    });
    this.services.GetVistaContratoContratista(ret[0].contratoId).subscribe((resp_1:any)=>{
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
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
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
