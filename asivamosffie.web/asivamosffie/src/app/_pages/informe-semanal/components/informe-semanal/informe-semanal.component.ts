import { Component, OnInit } from '@angular/core';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-informe-semanal',
  templateUrl: './informe-semanal.component.html',
  styleUrls: ['./informe-semanal.component.scss']
})
export class InformeSemanalComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}

  downloadPDF() {
    // html2canvas( document.getElementById( 'example-pdf' ) ).then(canvas => {
    //   const contentDataURL = canvas.toDataURL('image/jpeg', 1.0)

    //   let pdf = new jsPDF('l', 'cm', 'a4', false); //Generates PDF in landscape mode
    //   // let pdf = new jsPDF('p', 'cm', 'a4'); //Generates PDF in portrait mode
    //   pdf.addImage(contentDataURL, 'PNG', 0, 0, 29.8, 21.5, 'undefined', 'FAST')
    //   pdf.save('Filename.pdf');
    // });
    console.log(document.getElementById('example-pdf').innerHTML);
  }
}
