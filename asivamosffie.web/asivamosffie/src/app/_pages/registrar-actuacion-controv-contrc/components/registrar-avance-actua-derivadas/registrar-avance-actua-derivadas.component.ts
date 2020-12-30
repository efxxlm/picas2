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
    { name: 'Cumplida', value: '1' },
    { name: 'Incumplida', value: '2' },
  ];
  controversiaID: any;
  actuacionDerivadaID: any;
  controversia: any;
  constructor(private fb: FormBuilder,private router: Router, private conServices:ContractualControversyService,
    public dialog: MatDialog,
    public commonServices: CommonService,
    private activatedRoute: ActivatedRoute,)
     { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.controversiaID = param['id'];
      this.actuacionDerivadaID = param['editId'];
      this.conServices.GetActuacionSeguimientoById(this.controversiaID).subscribe(
        response=>{
          this.controversia=response;          
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
      seguimientoActuacionDerivadaId:0,
      controversiaActuacionId:this.controversia.controversiaActuacionId,
      esRequiereFiduciaria:false,
      fechaActuacionDerivada :this.addressForm.get("fechaActuacionDerivada").value,
      descripciondeActuacionAdelantada :this.addressForm.get("descripcionActuacionAdelantada").value,
      rutaSoporte :this.addressForm.get("urlSoporte").value,
      estadoActuacionDerivadaCodigo :this.addressForm.get("estadoActuacionDerivada").value,
      observaciones :this.addressForm.get("observaciones").value,}
    this.conServices.CreateEditarSeguimientoDerivado(obj).subscribe(
      response=>{
        this.controversia=response;          
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
          location.reload();             
      });
    }
  }
}
