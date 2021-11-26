using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit;
using MimeKit.Cryptography;

namespace CourseWorkMailClient.Infrastructure
{
    public class CryptoContext : CryptographyContext
    {
        public override string SignatureProtocol => throw new NotImplementedException();

        public override string EncryptionProtocol => throw new NotImplementedException();

        public override string KeyExchangeProtocol => throw new NotImplementedException();

        public override bool CanEncrypt(MailboxAddress mailbox, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override bool CanSign(MailboxAddress signer, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override MimeEntity Decrypt(Stream encryptedData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<MimeEntity> DecryptAsync(Stream encryptedData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override MimePart Encrypt(IEnumerable<MailboxAddress> recipients, Stream content, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<MimePart> EncryptAsync(IEnumerable<MailboxAddress> recipients, Stream content, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override MimePart Export(IEnumerable<MailboxAddress> mailboxes, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<MimePart> ExportAsync(IEnumerable<MailboxAddress> mailboxes, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override DigestAlgorithm GetDigestAlgorithm(string micalg)
        {
            throw new NotImplementedException();
        }

        public override string GetDigestAlgorithmName(DigestAlgorithm micalg)
        {
            throw new NotImplementedException();
        }

        public override void Import(Stream stream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task ImportAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override MimePart Sign(MailboxAddress signer, DigestAlgorithm digestAlgo, Stream content, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<MimePart> SignAsync(MailboxAddress signer, DigestAlgorithm digestAlgo, Stream content, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override bool Supports(string protocol)
        {
            throw new NotImplementedException();
        }

        public override DigitalSignatureCollection Verify(Stream content, Stream signatureData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<DigitalSignatureCollection> VerifyAsync(Stream content, Stream signatureData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
