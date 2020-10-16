import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-form-gestionar-fuentes',
  templateUrl: './form-gestionar-fuentes.component.html',
  styleUrls: ['./form-gestionar-fuentes.component.scss']
})
export class FormGestionarFuentesComponent implements OnInit {
  addressForm = this.fb.group({
    fuentes: this.fb.array([
      this.fb.group({
        fuentecampo: [null, Validators.required],
        saldoActual: [null, Validators.required],
        valorSolicitado: [null, Validators.compose([
          Validators.required, Validators.minLength(1), Validators.maxLength(20)])
        ],
        nuevoSaldo: [null, Validators.required]
      })
    ])
  });

  fuentesArray = [];
  solicitud: any;
  nombreAportante: any;
  valorAportante: any;
  llaveMen: any;
  tipoInterventor: any;
  departamento: any;
  municipio: any;
  institucion: any;
  sede: any;
  fuentesbase: any[];
  disponibilidadPresupuestalProyectoid: any;
  valorGestionado: any;

  constructor(
    private fb: FormBuilder, private fuenteFinanciacionService: FuenteFinanciacionService
    , private disponibilidadPresupuestalService: DisponibilidadPresupuestalService,
    public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) public data
  ) { }

  ngOnInit(): void { 
    console.log("Viene id"+this.data);
    console.log(this.data);
    this.solicitud=this.data.elemento.id;
    this.nombreAportante=this.data.elemento.nombreAportante;
    this.valorAportante=this.data.elemento.valorAportante;
    this.llaveMen=this.data.elemento.llaveMen;
    this.tipoInterventor=this.data.elemento.tipoInterventor;
    this.departamento=this.data.elemento.departamento;
    this.municipio=this.data.elemento.municipio;
    this.institucion=this.data.elemento.institucion;
    this.sede=this.data.elemento.sede;
    this.disponibilidadPresupuestalProyectoid=this.data.elemento.disponibilidadPresupuestalProyectoid;
    this.valorGestionado=this.data.elemento.valorGestionado;
    this.fuenteFinanciacionService.GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(this.disponibilidadPresupuestalProyectoid,this.data.elemento.id).subscribe(lista => {
      console.log(lista);
      this.fuentesbase=lista;
      lista.forEach(element => {
        this.fuentesArray.push({name:element.fuente,value:element.fuenteFinanciacionID});  
      });
      
    });
  }
  fuenteCambio(fuente:any)
  {
    let fuenteSeleccionada=this.fuentesbase.filter(x=>x.fuenteFinanciacionID==fuente.controls.fuentecampo.value);    
    fuente.get('saldoActual').setValue(fuenteSeleccionada[0].saldo_actual_de_la_fuente);
  }
  reste(fuente:any)
  {
    if(fuente.controls.saldoActual.value-fuente.controls.valorSolicitado.value<0)
    {
      let fuenteSeleccionada=this.fuentesbase.filter(x=>x.fuenteFinanciacionID==fuente.controls.fuentecampo.value);    
      this.openDialog("","El saldo actual de la fuente <b>"+fuenteSeleccionada[0].fuente+"</b> es menor al valor solicitado de la fuente, verifique por favor.");
      fuente.get('valorSolicitado').setValue(0);  
    }
    fuente.get('nuevoSaldo').setValue(fuente.controls.saldoActual.value-fuente.controls.valorSolicitado.value);
    
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialog=this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });

  }

  get fuentes() {
    return this.addressForm.get('fuentes') as FormArray;
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaFuente() {
    this.fuentes.push(this.crearFuente());
  }

  crearFuente() {
    return this.fb.group({
      fuentecampo: [null, Validators.required],
      saldoActual: [null, Validators.required],
      valorSolicitado: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(20)])
      ],
      nuevoSaldo: [null, Validators.required],
    });
  }

  onSubmit() {
    console.log(this.addressForm.controls.fuentes.value);
    let mensaje="";
    this.addressForm.controls.fuentes.value.forEach(fuente => {
      let CreateFinancialFundingGestion={
        FuenteFinanciacionId:fuente.fuentecampo,
        ValorSolicitado:fuente.valorSolicitado,
        DisponibilidadPresupuestalProyectoId:this.disponibilidadPresupuestalProyectoid};
      this.disponibilidadPresupuestalService.CreateFinancialFundingGestion(CreateFinancialFundingGestion).subscribe(result=>
        {
          console.log("Guardado");
          mensaje=result.message
        });
    });      
    this.openDialog('','<b>La información a sido guardad exitosamente.</b>');  
    
  }
}
