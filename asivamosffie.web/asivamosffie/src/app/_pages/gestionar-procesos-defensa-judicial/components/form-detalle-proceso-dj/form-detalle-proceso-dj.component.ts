import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DefensaJudicialService } from '../../../../core/_services/defensaJudicial/defensa-judicial.service';

@Component({
  selector: 'app-form-detalle-proceso-dj',
  templateUrl: './form-detalle-proceso-dj.component.html',
  styleUrls: ['./form-detalle-proceso-dj.component.scss']
})
export class FormDetalleProcesoDjComponent implements OnInit {
  addressForm = this.fb.group({
    departamentoInicio: [null, Validators.required],
    municipioInicio: [null, Validators.required],
    tipoAccion: [null, Validators.required],
    jurisdiccion: [null, Validators.required],
    pretensiones: [null, Validators.required],
    cuantiaPerjuicios: [null, Validators.required],
    requeridoParticipacionSupervisor: [null, Validators.required]
  });
  departamentoArray = [
  ];
  municipioArray = [
  ];
  tipoAccionArray = [
  ];
  jurisdiccionArray = [
  ];
  editorStyle = {
    height: '50px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  constructor(private fb: FormBuilder,public dialog: MatDialog, public commonService:CommonService
    ,private router: Router, public defensaService: DefensaJudicialService) { }

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;

  ngAfterViewInit(){
    this.cargarRegistro();
  }

  cargarRegistro() {
    //this.ngOnInit().then(() => {
      console.log("form");
      console.log(this.defensaJudicial);
      console.log(this.legitimacion);
      console.log(this.tipoProceso);      
      this.addressForm.get("tipoAccion").setValue(this.defensaJudicial.tipoAccionCodigo);
      this.addressForm.get("jurisdiccion").setValue(this.defensaJudicial.jurisdiccionCodigo);
      this.addressForm.get("pretensiones").setValue(this.defensaJudicial.pretensiones);
      this.addressForm.get("cuantiaPerjuicios").setValue(this.defensaJudicial.cuantiaPerjuicios);
      this.addressForm.get("requeridoParticipacionSupervisor").setValue(this.defensaJudicial.esRequiereSupervisor);
      

  }
  
  ngOnInit(): void {
    this.commonService.listaDepartamentos().subscribe(response=>{
      this.departamentoArray=response;
    });

    this.commonService.listaTipoAccionJudicial().subscribe(response=>{
      this.tipoAccionArray=response;
    });

    this.commonService.listaJurisdiccion().subscribe(response=>{
      this.jurisdiccionArray=response;
    });
    
  }

  getMunicipio(event: MatSelectChange) {
    this.commonService.listaMunicipiosByIdDepartamento(event.value).subscribe(respuesta => {
      this.municipioArray = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
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

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
        console.log("cerre ventana, redirecciono?"+id);
          if(id>0)
          {
            this.router.navigate(["/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial/"+id], {});
          }                  
      });
    }
  }

  onSubmit() {
    let defensaJudicial:DefensaJudicial={
      defensaJudicialId:this.defensaJudicial.defensaJudicialId,
      
      tipoProcesoCodigo:this.tipoProceso,
      numeroProceso:'',
      solicitudId:0,
      esLegitimacionActiva:this.legitimacion,
      localizacionIdMunicipio:this.addressForm.get("municipioInicio").value,
      tipoAccionCodigo:this.addressForm.get("tipoAccion").value,
      jurisdiccionCodigo:this.addressForm.get("jurisdiccion").value,
      pretensiones:this.addressForm.get("pretensiones").value,
      cuantiaPerjuicios:this.addressForm.get("cuantiaPerjuicios").value,
      esRequiereSupervisor:this.addressForm.get("requeridoParticipacionSupervisor").value,
      
    };
      console.log(defensaJudicial);
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial).subscribe(
        response=>{
          this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
        }
      );
    
  }

  onChangeMun()
  {
    this.commonService.listaMunicipiosByIdDepartamento(this.addressForm.get("departamentoInicio").value).subscribe(response=>{
      this.municipioArray=response;
    });
  }
}
