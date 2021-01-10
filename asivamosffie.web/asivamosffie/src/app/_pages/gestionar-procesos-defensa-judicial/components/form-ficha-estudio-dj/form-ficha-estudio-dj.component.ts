import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-ficha-estudio-dj',
  templateUrl: './form-ficha-estudio-dj.component.html',
  styleUrls: ['./form-ficha-estudio-dj.component.scss']
})
export class FormFichaEstudioDjComponent implements OnInit {

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
 
  addressForm = this.fb.group({
    antecedentes: [null, Validators.required],
    hechosRelevantes: [null, Validators.required],
    jurisprudenciaDoctrina: [null, Validators.required],
    decisionComite: [null, Validators.required],
    analisisJuridico: [null, Validators.required],
    recomendaciones: [null, Validators.required],
    procesoFichaComite: [null, Validators.required],
    fechaComiteDefensa : [null, Validators.required],
    recomendacionFinal : [null, Validators.required],
    aperturaFormalProceso: [null, Validators.required],
    tipoActuacionRecomendada: [null, Validators.required],
    actuacionRecomendadaAlComite: [null, Validators.required],
    urlSoporte: [null, Validators.required]
  });
  actuacionesArray = [
  ];
  constructor(  private fb: FormBuilder, public dialog: MatDialog, 
    public commonServices: CommonService,
    public defensaService: DefensaJudicialService,
    public judicialServices:DefensaJudicialService,
    private activatedRoute: ActivatedRoute,private router: Router) { }

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;

  ngAfterViewInit(){
    setTimeout(()=>{                       
      this.cargarRegistro();}, 5000);
  }

  cargarRegistro() {
    //this.ngOnInit().then(() => {
      console.log("form...");
      console.log(this.defensaJudicial);
      console.log(this.legitimacion);
      console.log(this.tipoProceso);      
      this.addressForm.get("antecedentes").setValue(this.defensaJudicial.fichaEstudio[0].antecedentes);
      this.addressForm.get("hechosRelevantes").setValue(this.defensaJudicial.fichaEstudio[0].hechosRelevantes);
      this.addressForm.get("jurisprudenciaDoctrina").setValue(this.defensaJudicial.fichaEstudio[0].jurisprudenciaDoctrina);
      this.addressForm.get("decisionComite").setValue(this.defensaJudicial.fichaEstudio[0].decisionComiteDirectrices);
      this.addressForm.get("analisisJuridico").setValue(this.defensaJudicial.fichaEstudio[0].analisisJuridico);
      this.addressForm.get("recomendaciones").setValue(this.defensaJudicial.fichaEstudio[0].recomendaciones);
      this.addressForm.get("procesoFichaComite").setValue(this.defensaJudicial.fichaEstudio[0].esPresentadoAnteComiteFfie);
      this.addressForm.get("fechaComiteDefensa").setValue(this.defensaJudicial.fichaEstudio[0].fechaComiteDefensa);
      this.addressForm.get("recomendacionFinal").setValue(this.defensaJudicial.fichaEstudio[0].recomendacionFinalComite);
      this.addressForm.get("aperturaFormalProceso").setValue(this.defensaJudicial.fichaEstudio[0].esAprobadoAperturaProceso);
      this.addressForm.get("tipoActuacionRecomendada").setValue(this.defensaJudicial.fichaEstudio[0].tipoActuacionCodigo);
      this.addressForm.get("actuacionRecomendadaAlComite").setValue(this.defensaJudicial.fichaEstudio[0].esActuacionTramiteComite);
      this.addressForm.get("urlSoporte").setValue(this.defensaJudicial.fichaEstudio[0].rutaSoporte);

  }

  ngOnInit(): void {
    this.commonServices.getTipoActuacion().subscribe(response=>{
      this.actuacionesArray=response;
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
        if(id>0 && this.defensaJudicial.defensaJudicialId==0)
        {
          this.router.navigate(["/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial/"+id], {});
        }                  
        else{
          location.reload();
        }                    
      });
    }
  }

  onSubmit() {
    let defensaJudicial=this.defensaJudicial;
    if(!this.defensaJudicial.defensaJudicialId||this.defensaJudicial.defensaJudicialId==0)
    {
      defensaJudicial={
        defensaJudicialId:this.defensaJudicial.defensaJudicialId,
        tipoProcesoCodigo:this.tipoProceso,
        esLegitimacionActiva:this.legitimacion,  
      };
    }

    defensaJudicial.fichaEstudio=[{
      antecedentes:this.addressForm.get("antecedentes").value,
      hechosRelevantes:this.addressForm.get("hechosRelevantes").value,
      jurisprudenciaDoctrina:this.addressForm.get("jurisprudenciaDoctrina").value,
      decisionComiteDirectrices:this.addressForm.get("decisionComite").value,
      analisisJuridico:this.addressForm.get("analisisJuridico").value,
      recomendaciones:this.addressForm.get("recomendaciones").value,
      esPresentadoAnteComiteFfie:this.addressForm.get("procesoFichaComite").value,      
      fechaComiteDefensa:this.addressForm.get("fechaComiteDefensa").value,
      recomendacionFinalComite:this.addressForm.get("recomendacionFinal").value,
      esAprobadoAperturaProceso:this.addressForm.get("aperturaFormalProceso").value,
      tipoActuacionCodigo:this.addressForm.get("tipoActuacionRecomendada").value,
      esActuacionTramiteComite:this.addressForm.get("actuacionRecomendadaAlComite").value,
      rutaSoporte:this.addressForm.get("urlSoporte").value,
    }];
    
      console.log(defensaJudicial);
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial).subscribe(
        response=>{
          this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
        }
      );
  }
}
