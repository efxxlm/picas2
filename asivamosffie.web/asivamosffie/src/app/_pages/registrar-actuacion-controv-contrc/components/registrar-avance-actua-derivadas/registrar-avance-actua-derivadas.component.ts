import { Component, OnInit } from '@angular/core';
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
export class RegistrarAvanceActuaDerivadasComponent implements OnInit {
  addressForm = this.fb.group({
    fechaActuacionDerivada: [null, Validators.required],
    descripcionActuacionAdelantada: [null, Validators.required],
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
  constructor(private fb: FormBuilder,private router: Router, private conServices:ContractualControversyService,
    public dialog: MatDialog,
    public commonServices: CommonService,
    private activatedRoute: ActivatedRoute,)
     { }

  ngOnInit(): void {
    this.commonServices.getEstadoActuacionDerivada().subscribe(
      response=>{
        this.estadoDerivadaArray=response;
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
              this.addressForm.get("descripcionActuacionAdelantada").setValue(this.actuacion.descripciondeActuacionAdelantada);
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
    let obj={
      seguimientoActuacionDerivadaId:this.actuacionDerivadaID,
      controversiaActuacionId:this.controversia.controversiaActuacionId,
      esRequiereFiduciaria:false,
      fechaActuacionDerivada :this.addressForm.get("fechaActuacionDerivada").value,
      descripciondeActuacionAdelantada :this.addressForm.get("descripcionActuacionAdelantada").value,
      rutaSoporte :this.addressForm.get("urlSoporte").value,
      estadoActuacionDerivadaCodigo :this.addressForm.get("estadoActuacionDerivada").value,
      observaciones :this.addressForm.get("observaciones").value,}
    this.conServices.CreateEditarSeguimientoDerivado(obj).subscribe(
      response=>{
        this.openDialog("",response.message,true);        
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
