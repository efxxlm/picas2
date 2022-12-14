import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-registrar-avance-actua-derivadas',
  templateUrl: './registrar-avance-actua-derivadas.component.html',
  styleUrls: ['./registrar-avance-actua-derivadas.component.scss']
})
export class RegistrarAvanceActuaDerivadasComponent implements OnInit, OnDestroy {
  addressForm = this.fb.group({
    fechaActuacionDerivada: [null, Validators.required],
    proximaActuacionRequerida: [null, Validators.required],
    estadoActuacionDerivada: [null, Validators.required],
    observaciones: [null, Validators.required],
    urlSoporte: [null, Validators.required]
  });
  editorStyle = {
    height: '25px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  estadoDerivadaArray = [
  ];
  controversiaID: any;
  actuacionDerivadaID: any;
  controversia: any;
  actuacion: any;
  seRealizoPeticion=false;
  estaEditando = false;
  ngOnDestroy(): void {
    if (this.addressForm.dirty === true  && this.seRealizoPeticion === false ) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.onSubmit();          
        }           
      });
    }
}

  constructor(private fb: FormBuilder,private router: Router, private conServices:ContractualControversyService,
    public dialog: MatDialog,
    public commonServices: CommonService,
    private activatedRoute: ActivatedRoute,)
     { }

  ngOnInit(): void {
    this.commonServices.getEstadoActuacionDerivada().subscribe(
      response=>{
        console.log(response);
        response.forEach(element => {
          if(element.codigo !== "3"){
            this.estadoDerivadaArray.push(element);
          }
        });
        //this.estadoDerivadaArray=response;
      }
    );
    this.activatedRoute.params.subscribe( param => {
      this.controversiaID = param['id'];
      this.actuacionDerivadaID = param['editId'];
      this.conServices.GetControversiaActuacionById(this.controversiaID).subscribe(
        response=>{
          this.controversia=response;    
          if(this.actuacionDerivadaID>0)
          {
            var seguimientos=this.controversia.controversiaActuacion.seguimientoActuacionDerivada;      
            this.actuacion=seguimientos.filter(x=>x.seguimientoActuacionDerivadaId==this.actuacionDerivadaID)[0];
            if(this.actuacion)
            {
              this.addressForm.get("fechaActuacionDerivada").setValue(this.actuacion.fechaActuacionDerivada);
              this.addressForm.get("proximaActuacionRequerida").setValue(this.actuacion.descripciondeActuacionAdelantada);
              this.addressForm.get("urlSoporte").setValue(this.actuacion.rutaSoporte);
              this.addressForm.get("estadoActuacionDerivada").setValue(this.actuacion.estadoActuacionDerivadaCodigo);
              this.addressForm.get("observaciones").setValue(this.actuacion.observaciones);
            }
          }
          

        }
      );
    });
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    console.log(e.editor.getLength()+" "+n);
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }
  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let obj={
      seguimientoActuacionDerivadaId:this.actuacionDerivadaID,
      controversiaActuacionId:this.controversia.controversiaActuacionId,
      esRequiereFiduciaria:false,
      fechaActuacionDerivada :this.addressForm.get("fechaActuacionDerivada").value,
      descripciondeActuacionAdelantada :this.addressForm.get("proximaActuacionRequerida").value,
      rutaSoporte :this.addressForm.get("urlSoporte").value,
      estadoActuacionDerivadaCodigo :this.addressForm.get("estadoActuacionDerivada").value,
      observaciones :this.addressForm.get("observaciones").value,}      
    this.conServices.CreateEditarSeguimientoDerivado(obj).subscribe(
      response=>{
        this.seRealizoPeticion=true;
        this.openDialog( '', `<b>${ response.message }</b>`,true);   
      }
    );

  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
        this.router.navigate(["/registrarActuacionesControversiasContractuales/actualizarTramite/"+this.controversiaID], {});          
      });
    }
  }
}
